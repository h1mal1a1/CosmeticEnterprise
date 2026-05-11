using CosmeticEnterpriseBack.Application.DTOs.Orders;
using CosmeticEnterpriseBack.Domain.Entities;

namespace CosmeticEnterpriseBack.Application.Interfaces;

public interface IOrderQueryBuilder
{
    void Normalize(GetOrdersQuery query);

    IQueryable<Orders> ApplyFilters(IQueryable<Orders> queryable, GetOrdersQuery query, bool allowUserFilter);
}