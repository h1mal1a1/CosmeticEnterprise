using CosmeticEnterpriseBack.Application.DTOs.Orders;
using CosmeticEnterpriseBack.Domain.Entities;

namespace CosmeticEnterpriseBack.Application.Validators;

public interface IOrderStatusTransitionValidator
{
    void Validate(Orders order, UpdateOrderStatusesRequest request);
}