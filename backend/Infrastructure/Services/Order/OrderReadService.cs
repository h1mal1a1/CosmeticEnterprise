using CosmeticEnterpriseBack.Application.DTOs.Orders;
using CosmeticEnterpriseBack.Application.Interfaces;
using CosmeticEnterpriseBack.Application.Mappers;
using CosmeticEnterpriseBack.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace CosmeticEnterpriseBack.Infrastructure.Services.Order;

public class OrderReadService(AppDbContext dbContext, IOrderMapper orderMapper, IOrderQueryBuilder orderQueryBuilder) : IOrderReadService
{
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
}