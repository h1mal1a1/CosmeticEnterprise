using CosmeticEnterpriseBack.Controllers.Base;
using CosmeticEnterpriseBack.DTO.FinishedProduct;
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
    public FinishedProductsController(
        ICrudService<
            FinishedProductResponse, 
            CreateFinishedProductRequest, 
            UpdateFinishedProductRequest, 
            long> crud) : base(crud)
    {
        
    }
}