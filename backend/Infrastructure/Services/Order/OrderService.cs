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
    IOrderStatusTransitionValidator orderStatusTransitionValidator, IOrderReturnUrlValidator orderReturnUrlValidator, 
    IOrderQueryBuilder orderQueryBuilder) : IOrderService
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

        var now = DateTime.UtcNow;

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var reservation = await orderStockService.ReserveCartItemsAsync([.. cart.Items], cancellationToken);

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
                TotalAmount = reservation.TotalAmount,
                DeliveryPrice = 0m,
                Comment = request.Comment,
                CreatedAtUtc = now,
                UpdatedAtUtc = now,
                OrderItemsList = [.. reservation.OrderItems]
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
        orderQueryBuilder.Normalize(query);

        var ordersQuery = dbContext.Orders
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.OrderItemsList)
            .Where(x => x.IdUser == userId)
            .AsQueryable();

        ordersQuery = orderQueryBuilder.ApplyFilters(ordersQuery, query, allowUserFilter: false);

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
        orderQueryBuilder.Normalize(query);

        var ordersQuery = dbContext.Orders
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.OrderItemsList)
            .AsQueryable();

        ordersQuery = orderQueryBuilder.ApplyFilters(ordersQuery, query, allowUserFilter: true);

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
}