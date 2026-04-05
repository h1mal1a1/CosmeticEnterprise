using CosmeticEnterpriseBack.Authorization;
using CosmeticEnterpriseBack.Controllers.Base;
using CosmeticEnterpriseBack.DTO.UnitOfMeasurement;
using CosmeticEnterpriseBack.Entities;
using CosmeticEnterpriseBack.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticEnterpriseBack.Controllers;

[ApiController]
[Authorize]
[Route("api/units-of-measurement")]
public class UnitsOfMeasurementController
    : CrudController<UnitOfMeasurementResponse, CreateUnitOfMeasurementRequest, UpdateUnitOfMeasurementRequest, long>
{
    public UnitsOfMeasurementController(ICrudServiceFactory crudServiceFactory)
        : base(
            crudServiceFactory.Create<
                UnitsOfMeasurement,
                long,
                CreateUnitOfMeasurementRequest,
                UpdateUnitOfMeasurementRequest,
                UnitOfMeasurementResponse>(ResourceType.UnitOfMeasurement))
    {
    }
}