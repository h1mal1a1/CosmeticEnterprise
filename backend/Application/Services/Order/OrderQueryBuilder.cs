using CosmeticEnterpriseBack.Application.DTOs.Orders;
using CosmeticEnterpriseBack.Application.Interfaces;
using CosmeticEnterpriseBack.Domain.Entities;

namespace CosmeticEnterpriseBack.Application.Services.Order;

public class OrderQueryBuilder : IOrderQueryBuilder
{
    public void Normalize(GetOrdersQuery query)
    {
        if (query.Page <= 0)
            query.Page = 1;

        if (query.PageSize <= 0)
            query.PageSize = 20;

        if (query.PageSize > 100)
            query.PageSize = 100;
    }

    public IQueryable<Orders> ApplyFilters(IQueryable<Orders> queryable, GetOrdersQuery query, bool allowUserFilter)
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
}