using CosmeticEnterpriseBack.Api.DTOs.UnitOfMeasurement;
using CosmeticEnterpriseBack.Infrastructure.Interfaces;

namespace CosmeticEnterpriseBack.Mappers.UnitsOfMeasurement;

public class UnitOfMeasurementUpdateMapper
    : IUpdateMapper<Domain.Entities.UnitsOfMeasurement, UpdateUnitOfMeasurementRequest>
{
    public void Map(UpdateUnitOfMeasurementRequest source, Domain.Entities.UnitsOfMeasurement entity)
    {
        entity.Name = source.Name;
    }
}