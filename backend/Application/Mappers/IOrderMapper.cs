using CosmeticEnterpriseBack.Application.DTOs.Orders;
using CosmeticEnterpriseBack.Domain.Entities;

namespace CosmeticEnterpriseBack.Application.Mappers;

public interface IOrderMapper
{
    OrderListItemResponse ToListItemResponse(Orders order);

    OrderResponse ToResponse(Orders order);
}