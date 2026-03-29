using CosmeticEnterpriseBack.DTO.UnitOfMeasurement;
using CosmeticEnterpriseBack.Interfaces;

namespace CosmeticEnterpriseBack.Mappers.UnitsOfMeasurement;

public class UnitOfMeasurementUpdateMapper
    : IUpdateMapper<Entities.UnitsOfMeasurement, UpdateUnitOfMeasurementRequest>
{
    public void Map(UpdateUnitOfMeasurementRequest source, Entities.UnitsOfMeasurement entity)
    {
        entity.Name = source.Name;
    }
}