using CosmeticEnterpriseBack.DTO.UnitOfMeasurement;
using CosmeticEnterpriseBack.Interfaces;

namespace CosmeticEnterpriseBack.Mappers.UnitOfMeasurement;

public class UnitOfMeasurementResponseMapper
    : IResponseMapper<Entities.UnitsOfMeasurement, UnitOfMeasurementResponse>
{
    public UnitOfMeasurementResponse Map(Entities.UnitsOfMeasurement source)
    {
        return new UnitOfMeasurementResponse
        {
            Id = source.Id,
            Name = source.Name
        };
    }
}