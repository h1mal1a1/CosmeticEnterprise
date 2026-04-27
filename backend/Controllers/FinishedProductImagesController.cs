using CosmeticEnterpriseBack.DTO.FinishedProductImages;
using CosmeticEnterpriseBack.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticEnterpriseBack.Controllers;

[ApiController]
[Authorize]
[Route("api/finished-products/{finishedProductId:long}/images")]
public class FinishedProductImagesController : ControllerBase
{
    private readonly IFinishedProductImageService _finishedProductImageService;

    public FinishedProductImagesController(IFinishedProductImageService finishedProductImageService) =>
        _finishedProductImageService = finishedProductImageService;

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<IReadOnlyList<FinishedProductImageResponse>>> Upload([FromRoute] long finishedProductId,
        [FromForm] UploadFinishedProductImageRequest request, CancellationToken cancellationToken)
    {
        var result = await _finishedProductImageService.UploadAsync(
            finishedProductId,
            request,
            cancellationToken);

        return Ok(result);
    }

    [HttpPatch("{imageId:long}/main")]
    public async Task<ActionResult<IReadOnlyList<FinishedProductImageResponse>>> SetMain([FromRoute] long finishedProductId,
        [FromRoute] long imageId, CancellationToken cancellationToken)
    {
        var result = await _finishedProductImageService.SetMainAsync(
            finishedProductId,
            imageId,
            cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{imageId:long}")]
    public async Task<IActionResult> Delete([FromRoute] long finishedProductId, [FromRoute] long imageId, CancellationToken cancellationToken)
    {
        await _finishedProductImageService.DeleteAsync(
            finishedProductId,
            imageId,
            cancellationToken);

        return NoContent();
    }
}