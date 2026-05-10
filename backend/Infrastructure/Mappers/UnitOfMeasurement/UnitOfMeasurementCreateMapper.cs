using CosmeticEnterpriseBack.Api.DTOs.UnitOfMeasurement;
using CosmeticEnterpriseBack.Infrastructure.Interfaces;
namespace CosmeticEnterpriseBack.Infrastructure.Mappers.UnitOfMeasurement;
public class UnitOfMeasurementCreateMapper
    : ICreateMapper<Domain.Entities.UnitsOfMeasurement, CreateUnitOfMeasurementRequest>
{
    public Domain.Entities.UnitsOfMeasurement Map(CreateUnitOfMeasurementRequest source)
    {
        return new Domain.Entities.UnitsOfMeasurement
        {
            Name = source.Name
        };
    }
}