using CosmeticEnterpriseBack.Authorization;
using CosmeticEnterpriseBack.Controllers.Base;
using CosmeticEnterpriseBack.DTO.FinishedProduct;
using CosmeticEnterpriseBack.Entities;
using CosmeticEnterpriseBack.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticEnterpriseBack.Controllers;

[Route("api/finished-products")]
public class FinishedProductsController :
    CrudController<
        FinishedProductResponse,
        CreateFinishedProductRequest,
        UpdateFinishedProductRequest,
        long>
{
    public FinishedProductsController(ICrudServiceFactory crudFactory)
        : base(
            crudFactory.Create<
                FinishedProducts,
                long,
                CreateFinishedProductRequest,
                UpdateFinishedProductRequest,
                FinishedProductResponse>(ResourceType.FinishedProduct))
    {

    }
}