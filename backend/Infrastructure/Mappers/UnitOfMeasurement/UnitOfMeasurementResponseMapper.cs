using CosmeticEnterpriseBack.Api.DTOs.UnitOfMeasurement;
using CosmeticEnterpriseBack.Infrastructure.Interfaces;

namespace CosmeticEnterpriseBack.Infrastructure.Mappers.UnitOfMeasurement;

public class UnitOfMeasurementResponseMapper
    : IResponseMapper<Domain.Entities.UnitsOfMeasurement, UnitOfMeasurementResponse>
{
    public UnitOfMeasurementResponse Map(Domain.Entities.UnitsOfMeasurement source)
    {
        return new UnitOfMeasurementResponse
        {
            Id = source.Id,
            Name = source.Name
        };
    }
}