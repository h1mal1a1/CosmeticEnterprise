using CosmeticEnterpriseBack.DTO.UnitOfMeasurement;
using CosmeticEnterpriseBack.Interfaces;
namespace CosmeticEnterpriseBack.Mappers.UnitOfMeasurement;

public class UnitOfMeasurementCreateMapper
    : ICreateMapper<Entities.UnitsOfMeasurement, CreateUnitOfMeasurementRequest>
{
    public Entities.UnitsOfMeasurement Map(CreateUnitOfMeasurementRequest source)
    {
        return new Entities.UnitsOfMeasurement
        {
            Name = source.Name
        };
    }
}