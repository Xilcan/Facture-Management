using Bussines.Dto.ProductCategory;
using Bussines.Services.Interfaces;
using Interface.ExceptionsHandling.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductCategoryController : ControllerBase
{
    private readonly IProductCategoryService _productCategoryService;

    public ProductCategoryController(IProductCategoryService productCategoryService)
    {
        ArgumentNullException.ThrowIfNull(productCategoryService);
        _productCategoryService = productCategoryService;
    }

    // GET: api/<ProductCategoryController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BriefProductCategoryGet>>> Get()
    {
        var userCompanyIdString = HttpContext.Items["CompanyId"]?.ToString();

        return userCompanyIdString == null || !Guid.TryParse(userCompanyIdString, out Guid userCompanyId)
            ? (ActionResult<IEnumerable<BriefProductCategoryGet>>)Forbid()
            : (ActionResult<IEnumerable<BriefProductCategoryGet>>)Ok(await _productCategoryService.GetAllAsync(userCompanyId));
    }

    // GET api/<ProductCategoryController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<FullProductCategoryGet>> Get(Guid id)
    {
        var userCompanyIdString = HttpContext.Items["CompanyId"]?.ToString();

        return userCompanyIdString == null || !Guid.TryParse(userCompanyIdString, out Guid userCompanyId)
            ? (ActionResult<FullProductCategoryGet>)Forbid()
            : (ActionResult<FullProductCategoryGet>)(id == Guid.Empty
            ? throw new BadRequestException($"Null or Empty argument {nameof(id)}")
            : Ok(await _productCategoryService.GetByIdAsync(id, userCompanyId)));
    }

    // POST api/<ProductCategoryController>
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] ProductCategoryPost request)
    {
        if (request == null)
        {
            throw new BadRequestException($"Null or Empty argument {nameof(request)}");
        }

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            throw new BadRequestException("Validation error: \n" + errors);
        }

        var userCompanyIdString = HttpContext.Items["CompanyId"]?.ToString();

        if (userCompanyIdString == null || !Guid.TryParse(userCompanyIdString, out Guid userCompanyId))
        {
            return Forbid();
        }

        await _productCategoryService.AddAsync(request, userCompanyId);
        return Created();
    }

    // PUT api/<ProductCategoryController>/5
    [HttpPut]
    public async Task<ActionResult> Put([FromBody] BriefProductCategoryGet request)
    {
        if (request == null)
        {
            throw new BadRequestException($"Null or Empty argument {nameof(request)}");
        }

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            throw new BadRequestException("Validation error: \n" + errors);
        }

        var userCompanyIdString = HttpContext.Items["CompanyId"]?.ToString();

        if (userCompanyIdString == null || !Guid.TryParse(userCompanyIdString, out Guid userCompanyId))
        {
            return Forbid();
        }

        await _productCategoryService.UpdateAsync(request, userCompanyId);
        return Ok();
    }

    // DELETE api/<ProductCategoryController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new BadRequestException($"Null or Empty argument {nameof(id)}");
        }

        var userCompanyIdString = HttpContext.Items["CompanyId"]?.ToString();

        if (userCompanyIdString == null || !Guid.TryParse(userCompanyIdString, out Guid userCompanyId))
        {
            return Forbid();
        }

        await _productCategoryService.DeleteAsync(id, userCompanyId);
        return Ok();
    }
}
