using CosmeticEnterpriseBack.Infrastructure.Persistence.Data;
using CosmeticEnterpriseBack.Domain.Entities;
using CosmeticEnterpriseBack.Domain.Enums;
using CosmeticEnterpriseBack.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using CosmeticEnterpriseBack.Application.DTOs.Orders;
using CosmeticEnterpriseBack.Application.Mappers;
using CosmeticEnterpriseBack.Application.Validators;

namespace CosmeticEnterpriseBack.Infrastructure.Services.Order;

public class OrderService(AppDbContext dbContext, IOrderMapper orderMapper, IOrderStockService orderStockService,
    IOrderStatusTransitionValidator orderStatusTransitionValidator, IOrderReturnUrlValidator orderReturnUrlValidator) : IOrderService
{
    private const string WebsiteSalesChannelName = "Website";

    public async Task<OrderResponse> CreateOrderFromCartAsync(long userId, CreateOrderRequest request, CancellationToken cancellationToken)
    {
        orderReturnUrlValidator.Validate(request.ReturnUrl);

        var userAddress = await dbContext.UserAddresses
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == request.IdUserAddress && x.IdUser == userId,
                cancellationToken);

        if (userAddress is null)
            throw new KeyNotFoundException("User address not found.");

        var salesChannel = await dbContext.SalesChannels
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == WebsiteSalesChannelName, cancellationToken);

        if (salesChannel is null)
            throw new KeyNotFoundException("Website sales channel not found.");

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

            var orderStatus = OrderStatus.Created;

            var order = new Orders
            {
                IdUser = userId,
                IdUserAddress = request.IdUserAddress,
                IdSalesChannel = salesChannel.Id,
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
            .Include(x => x.User)
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
            Items = orders.Select(orderMapper.ToListItemResponse).ToList(),
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
            .Include(x => x.User)
            .Include(x => x.UserAddress)
            .Include(x => x.OrderItemsList)
                .ThenInclude(x => x.FinishedProducts)
            .FirstOrDefaultAsync(
                x => x.Id == orderId && x.IdUser == userId,
                cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Order not found.");

        return orderMapper.ToResponse(order);
    }

    public async Task<OrderResponse> CancelMyOrderAsync(long userId, long orderId, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .Include(x => x.User)
            .Include(x => x.UserAddress)
            .Include(x => x.OrderItemsList)
                .ThenInclude(x => x.FinishedProducts)
            .FirstOrDefaultAsync(
                x => x.Id == orderId && x.IdUser == userId,
                cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Order not found.");

        if (order.OrderStatus == OrderStatus.Cancelled)
            return orderMapper.ToResponse(order);

        if (order.OrderStatus == OrderStatus.Completed)
            throw new InvalidOperationException("Completed order cannot be cancelled.");

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await orderStockService.ReleaseReserveAsync(order, cancellationToken);

            order.OrderStatus = OrderStatus.Cancelled;
            order.DeliveryStatus = DeliveryStatus.Cancelled;
            order.UpdatedAtUtc = DateTime.UtcNow;

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return orderMapper.ToResponse(order);
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
            .Include(x => x.User)
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
            Items = orders.Select(orderMapper.ToListItemResponse).ToList(),
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
            .Include(x => x.User)
            .Include(x => x.UserAddress)
            .Include(x => x.OrderItemsList)
                .ThenInclude(x => x.FinishedProducts)
            .FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Order not found.");

        return orderMapper.ToResponse(order);
    }

    public async Task<OrderResponse> UpdateOrderStatusesAsync(long orderId, UpdateOrderStatusesRequest request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .Include(x => x.User)
            .Include(x => x.UserAddress)
            .Include(x => x.OrderItemsList)
                .ThenInclude(x => x.FinishedProducts)
            .FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Order not found.");

        orderStatusTransitionValidator.Validate(order, request);

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
                await orderStockService.ReleaseReserveAsync(order, cancellationToken);

            if (becomesCompleted)
                await orderStockService.ConsumeReservedStockAsync(order, cancellationToken);

            order.OrderStatus = request.OrderStatus;
            order.DeliveryStatus = request.DeliveryStatus;
            order.PaymentStatus = request.PaymentStatus;
            order.UpdatedAtUtc = DateTime.UtcNow;

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return orderMapper.ToResponse(order);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
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
}