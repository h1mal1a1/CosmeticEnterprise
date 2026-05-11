using CosmeticEnterpriseBack.Api.Controllers.Base;
using CosmeticEnterpriseBack.Application.DTOs.UnitOfMeasurement;
using CosmeticEnterpriseBack.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticEnterpriseBack.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/units-of-measurement")]
public class UnitsOfMeasurementController(ICrudServiceFactory crudFactory)
    : CrudController<UnitOfMeasurementResponse, CreateUnitOfMeasurementRequest, UpdateUnitOfMeasurementRequest, long>
    (crudFactory.CreateUnitsOfMeasurementService())
{
}