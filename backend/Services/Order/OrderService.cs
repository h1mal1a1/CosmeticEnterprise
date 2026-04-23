using CosmeticEnterpriseBack.Data;
using CosmeticEnterpriseBack.DTO.Orders;
using CosmeticEnterpriseBack.Entities;
using CosmeticEnterpriseBack.Enums;
using CosmeticEnterpriseBack.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CosmeticEnterpriseBack.Services.Order;

public class OrderService(AppDbContext dbContext) : IOrderService
{
    public async Task<OrderResponse> CreateOrderFromCartAsync(long userId, CreateOrderRequest request, CancellationToken cancellationToken)
    {
        ValidateReturnUrl(request.ReturnUrl);

        var userAddress = await dbContext.UserAddresses
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == request.IdUserAddress && x.IdUser == userId,
                cancellationToken);

        if (userAddress is null)
            throw new KeyNotFoundException("User address not found.");

        var salesChannelExists = await dbContext.SalesChannels
            .AsNoTracking()
            .AnyAsync(x => x.Id == request.IdSalesChannel, cancellationToken);

        if (!salesChannelExists)
            throw new KeyNotFoundException("Sales channel not found.");

        var cart = await dbContext.ShoppingCarts
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.IdUser == userId, cancellationToken);

        if (cart is null || cart.Items.Count == 0)
            throw new InvalidOperationException("Shopping cart is empty.");

        var cartItemProductIds = cart.Items
            .Select(x => x.IdFinishedProduct)
            .Distinct()
            .ToList();

        var products = await dbContext.FinishedProducts
            .Where(x => cartItemProductIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        var leftovers = await dbContext.LeftoversInWarehouses
            .Where(x => cartItemProductIds.Contains(x.IdFinishedProduct))
            .ToListAsync(cancellationToken);

        var now = DateTime.UtcNow;

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            decimal totalAmount = 0m;
            var orderItems = new List<OrderItems>();

            foreach (var cartItem in cart.Items)
            {
                if (!products.TryGetValue(cartItem.IdFinishedProduct, out var product))
                    throw new KeyNotFoundException(
                        $"Finished product with id {cartItem.IdFinishedProduct} not found.");

                var productLeftovers = leftovers
                    .Where(x => x.IdFinishedProduct == cartItem.IdFinishedProduct)
                    .OrderBy(x => x.Id)
                    .ToList();

                var availableQuantity = productLeftovers.Sum(x => x.Quantity - x.ReservedQuantity);

                if (availableQuantity < cartItem.Quantity)
                {
                    throw new InvalidOperationException(
                        $"Not enough stock for product '{product.Name}'. Available: {availableQuantity}, requested: {cartItem.Quantity}.");
                }

                var quantityToReserve = cartItem.Quantity;

                foreach (var leftover in productLeftovers)
                {
                    var freeQuantity = leftover.Quantity - leftover.ReservedQuantity;

                    if (freeQuantity <= 0)
                        continue;

                    var reserveFromCurrent = Math.Min(freeQuantity, quantityToReserve);
                    leftover.ReservedQuantity += reserveFromCurrent;
                    quantityToReserve -= reserveFromCurrent;

                    if (quantityToReserve == 0)
                        break;
                }

                var unitPrice = product.Price;
                var lineTotal = unitPrice * cartItem.Quantity;

                totalAmount += lineTotal;

                orderItems.Add(new OrderItems
                {
                    IdFinishedProduct = cartItem.IdFinishedProduct,
                    Quantity = cartItem.Quantity,
                    UnitPrice = unitPrice,
                    LineTotal = lineTotal
                });
            }

            var orderStatus = request.PaymentType == PaymentType.Immediate
                ? OrderStatus.AwaitingPayment
                : OrderStatus.Created;

            var order = new Orders
            {
                IdUser = userId,
                IdUserAddress = request.IdUserAddress,
                IdSalesChannel = request.IdSalesChannel,
                OrderStatus = orderStatus,
                DeliveryStatus = DeliveryStatus.Pending,
                PaymentType = request.PaymentType,
                PaymentMethod = request.PaymentMethod,
                PaymentStatus = PaymentStatus.Pending,
                TotalAmount = totalAmount,
                DeliveryPrice = 0m,
                Comment = request.Comment,
                CreatedAtUtc = now,
                UpdatedAtUtc = now,
                OrderItemsList = orderItems
            };

            dbContext.Orders.Add(order);

            dbContext.ShoppingCartItems.RemoveRange(cart.Items);
            cart.UpdatedAtUtc = now;

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return await GetMyOrderByIdAsync(userId, order.Id, cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<PagedResult<OrderListItemResponse>> GetMyOrdersAsync(long userId, GetOrdersQuery query, CancellationToken cancellationToken)
    {
        NormalizeQuery(query);

        var ordersQuery = dbContext.Orders
            .AsNoTracking()
            .Include(x => x.OrderItemsList)
            .Where(x => x.IdUser == userId)
            .AsQueryable();

        ordersQuery = ApplyFilters(ordersQuery, query, allowUserFilter: false);

        var totalCount = await ordersQuery.CountAsync(cancellationToken);

        var orders = await ordersQuery
            .OrderByDescending(x => x.CreatedAtUtc)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<OrderListItemResponse>
        {
            Items = orders.Select(MapToListItemResponse).ToList(),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize)
        };
    }

    public async Task<OrderResponse> GetMyOrderByIdAsync(long userId, long orderId, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .AsNoTracking()
            .Include(x => x.OrderItemsList)
                .ThenInclude(x => x.FinishedProducts)
            .FirstOrDefaultAsync(
                x => x.Id == orderId && x.IdUser == userId,
                cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Order not found.");

        return MapToResponse(order);
    }

    public async Task<OrderResponse> CancelMyOrderAsync(long userId, long orderId, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .Include(x => x.OrderItemsList)
                .ThenInclude(x => x.FinishedProducts)
            .FirstOrDefaultAsync(
                x => x.Id == orderId && x.IdUser == userId,
                cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Order not found.");

        if (order.OrderStatus == OrderStatus.Cancelled)
            return MapToResponse(order);

        if (order.OrderStatus == OrderStatus.Completed)
            throw new InvalidOperationException("Completed order cannot be cancelled.");

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await ReleaseReserveAsync(order, cancellationToken);

            order.OrderStatus = OrderStatus.Cancelled;
            order.DeliveryStatus = DeliveryStatus.Cancelled;
            order.UpdatedAtUtc = DateTime.UtcNow;

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return MapToResponse(order);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<PagedResult<OrderListItemResponse>> GetAllOrdersAsync(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        NormalizeQuery(query);

        var ordersQuery = dbContext.Orders
            .AsNoTracking()
            .Include(x => x.OrderItemsList)
            .AsQueryable();

        ordersQuery = ApplyFilters(ordersQuery, query, allowUserFilter: true);

        var totalCount = await ordersQuery.CountAsync(cancellationToken);

        var orders = await ordersQuery
            .OrderByDescending(x => x.CreatedAtUtc)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<OrderListItemResponse>
        {
            Items = orders.Select(MapToListItemResponse).ToList(),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize)
        };
    }

    public async Task<OrderResponse> GetOrderByIdAsync(long orderId, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .AsNoTracking()
            .Include(x => x.OrderItemsList)
                .ThenInclude(x => x.FinishedProducts)
            .FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Order not found.");

        return MapToResponse(order);
    }

    public async Task<OrderResponse> UpdateOrderStatusesAsync(long orderId, UpdateOrderStatusesRequest request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .Include(x => x.OrderItemsList)
                .ThenInclude(x => x.FinishedProducts)
            .FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Order not found.");

        ValidateStatusTransition(order, request);

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var becomesCancelled =
                order.OrderStatus != OrderStatus.Cancelled &&
                request.OrderStatus == OrderStatus.Cancelled;

            var becomesCompleted =
                order.OrderStatus != OrderStatus.Completed &&
                request.OrderStatus == OrderStatus.Completed;

            if (becomesCancelled)
                await ReleaseReserveAsync(order, cancellationToken);

            if (becomesCompleted)
                await ConsumeReservedStockAsync(order, cancellationToken);

            order.OrderStatus = request.OrderStatus;
            order.DeliveryStatus = request.DeliveryStatus;
            order.PaymentStatus = request.PaymentStatus;
            order.UpdatedAtUtc = DateTime.UtcNow;

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return MapToResponse(order);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task ReleaseReserveAsync(Orders order, CancellationToken cancellationToken)
    {
        var orderProductIds = order.OrderItemsList
            .Select(x => x.IdFinishedProduct)
            .Distinct()
            .ToList();

        var leftovers = await dbContext.LeftoversInWarehouses
            .Where(x => orderProductIds.Contains(x.IdFinishedProduct))
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        foreach (var orderItem in order.OrderItemsList)
        {
            var quantityToRelease = orderItem.Quantity;

            var productLeftovers = leftovers
                .Where(x => x.IdFinishedProduct == orderItem.IdFinishedProduct)
                .OrderByDescending(x => x.Id)
                .ToList();

            var reservedTotal = productLeftovers.Sum(x => x.ReservedQuantity);

            if (reservedTotal < quantityToRelease)
            {
                throw new InvalidOperationException(
                    $"Cannot release reserve for product id {orderItem.IdFinishedProduct}. Reserved: {reservedTotal}, expected: {quantityToRelease}.");
            }

            foreach (var leftover in productLeftovers)
            {
                if (leftover.ReservedQuantity <= 0)
                    continue;

                var releaseFromCurrent = Math.Min(leftover.ReservedQuantity, quantityToRelease);
                leftover.ReservedQuantity -= releaseFromCurrent;
                quantityToRelease -= releaseFromCurrent;

                if (quantityToRelease == 0)
                    break;
            }
        }
    }

    private async Task ConsumeReservedStockAsync(Orders order, CancellationToken cancellationToken)
    {
        var orderProductIds = order.OrderItemsList
            .Select(x => x.IdFinishedProduct)
            .Distinct()
            .ToList();

        var leftovers = await dbContext.LeftoversInWarehouses
            .Where(x => orderProductIds.Contains(x.IdFinishedProduct))
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        foreach (var orderItem in order.OrderItemsList)
        {
            var quantityToConsume = orderItem.Quantity;

            var productLeftovers = leftovers
                .Where(x => x.IdFinishedProduct == orderItem.IdFinishedProduct)
                .OrderBy(x => x.Id)
                .ToList();

            var reservedTotal = productLeftovers.Sum(x => x.ReservedQuantity);

            if (reservedTotal < quantityToConsume)
            {
                throw new InvalidOperationException(
                    $"Cannot complete order for product id {orderItem.IdFinishedProduct}. Reserved: {reservedTotal}, expected: {quantityToConsume}.");
            }

            foreach (var leftover in productLeftovers)
            {
                if (leftover.ReservedQuantity <= 0 || leftover.Quantity <= 0)
                    continue;

                var consumeFromCurrent = Math.Min(
                    Math.Min(leftover.ReservedQuantity, leftover.Quantity),
                    quantityToConsume);

                leftover.ReservedQuantity -= consumeFromCurrent;
                leftover.Quantity -= consumeFromCurrent;
                quantityToConsume -= consumeFromCurrent;

                if (quantityToConsume == 0)
                    break;
            }

            if (quantityToConsume > 0)
            {
                throw new InvalidOperationException(
                    $"Unable to fully consume reserved stock for product id {orderItem.IdFinishedProduct}.");
            }
        }
    }

    private static IQueryable<Orders> ApplyFilters(IQueryable<Orders> queryable, GetOrdersQuery query, bool allowUserFilter)
    {
        if (query.OrderStatus.HasValue)
            queryable = queryable.Where(x => x.OrderStatus == query.OrderStatus.Value);

        if (query.DeliveryStatus.HasValue)
            queryable = queryable.Where(x => x.DeliveryStatus == query.DeliveryStatus.Value);

        if (query.PaymentStatus.HasValue)
            queryable = queryable.Where(x => x.PaymentStatus == query.PaymentStatus.Value);

        if (allowUserFilter && query.IdUser.HasValue)
            queryable = queryable.Where(x => x.IdUser == query.IdUser.Value);

        return queryable;
    }

    private static void NormalizeQuery(GetOrdersQuery query)
    {
        if (query.Page <= 0)
            query.Page = 1;

        if (query.PageSize <= 0)
            query.PageSize = 20;

        if (query.PageSize > 100)
            query.PageSize = 100;
    }

    private static OrderListItemResponse MapToListItemResponse(Orders order)
    {
        return new OrderListItemResponse
        {
            Id = order.Id,
            IdUser = order.IdUser,
            IdUserAddress = order.IdUserAddress,
            IdSalesChannel = order.IdSalesChannel,
            OrderStatus = order.OrderStatus,
            DeliveryStatus = order.DeliveryStatus,
            PaymentType = order.PaymentType,
            PaymentMethod = order.PaymentMethod,
            PaymentStatus = order.PaymentStatus,
            TotalAmount = order.TotalAmount,
            DeliveryPrice = order.DeliveryPrice,
            TotalItemsQuantity = order.OrderItemsList.Sum(x => x.Quantity),
            CreatedAtUtc = order.CreatedAtUtc,
            UpdatedAtUtc = order.UpdatedAtUtc
        };
    }

    private static OrderResponse MapToResponse(Orders order)
    {
        return new OrderResponse
        {
            Id = order.Id,
            IdUser = order.IdUser,
            IdUserAddress = order.IdUserAddress,
            IdSalesChannel = order.IdSalesChannel,
            OrderStatus = order.OrderStatus,
            DeliveryStatus = order.DeliveryStatus,
            PaymentType = order.PaymentType,
            PaymentMethod = order.PaymentMethod,
            PaymentStatus = order.PaymentStatus,
            TotalAmount = order.TotalAmount,
            DeliveryPrice = order.DeliveryPrice,
            Comment = order.Comment,
            CreatedAtUtc = order.CreatedAtUtc,
            UpdatedAtUtc = order.UpdatedAtUtc,
            Items = order.OrderItemsList
                .OrderBy(x => x.Id)
                .Select(x => new OrderItemResponse
                {
                    Id = x.Id,
                    IdFinishedProduct = x.IdFinishedProduct,
                    ProductName = x.FinishedProducts.Name,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    LineTotal = x.LineTotal
                })
                .ToList()
        };
    }

    private static void ValidateReturnUrl(string? returnUrl)
    {
        if (string.IsNullOrWhiteSpace(returnUrl))
            return;

        if (!Uri.TryCreate(returnUrl, UriKind.Absolute, out var uri))
            throw new ArgumentException("ReturnUrl must be an absolute URL.");

        if (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp)
            throw new ArgumentException("ReturnUrl must use http or https.");
    }

    private static void ValidateStatusTransition(Orders order, UpdateOrderStatusesRequest request)
    {
        if (order.OrderStatus == OrderStatus.Cancelled || order.OrderStatus == OrderStatus.Completed)
        {
            throw new InvalidOperationException(
                "Cancelled or completed order cannot be changed.");
        }

        if (request.PaymentStatus == PaymentStatus.Paid &&
            request.OrderStatus == OrderStatus.Created)
        {
            throw new InvalidOperationException(
                "Paid order cannot remain in Created status.");
        }

        if (request.DeliveryStatus == DeliveryStatus.Delivered &&
            request.OrderStatus != OrderStatus.Completed)
        {
            throw new InvalidOperationException(
                "Delivered order must have Completed order status.");
        }

        if (request.OrderStatus == OrderStatus.Cancelled &&
            request.DeliveryStatus != DeliveryStatus.Cancelled)
        {
            throw new InvalidOperationException(
                "Cancelled order must have Cancelled delivery status.");
        }

        if (request.OrderStatus == OrderStatus.Completed &&
            request.PaymentStatus != PaymentStatus.Paid)
        {
            throw new InvalidOperationException(
                "Completed order must have Paid payment status.");
        }
    }
}