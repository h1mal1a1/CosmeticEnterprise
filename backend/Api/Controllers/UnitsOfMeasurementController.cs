using CosmeticEnterpriseBack.Api.Controllers.Base;
using CosmeticEnterpriseBack.Application.DTOs.UnitOfMeasurement;
using CosmeticEnterpriseBack.Domain.Entities;
using CosmeticEnterpriseBack.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CosmeticEnterpriseBack.Application.Authorization;

namespace CosmeticEnterpriseBack.Api.Controllers;

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