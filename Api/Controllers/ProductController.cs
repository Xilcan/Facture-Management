using Bussines.Dto.Product;
using Bussines.Services.Interfaces;
using Interface.ExceptionsHandling.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        ArgumentNullException.ThrowIfNull(productService);
        _productService = productService;
    }

    // GET: api/<ProductController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BriefProductGet>>> GetAll([FromQuery] Guid? categoryId)
    {
        var userCompanyIdString = HttpContext.Items["CompanyId"]?.ToString();

        return userCompanyIdString == null || !Guid.TryParse(userCompanyIdString, out Guid userCompanyId)
            ? (ActionResult<IEnumerable<BriefProductGet>>)Forbid()
            : (ActionResult<IEnumerable<BriefProductGet>>)Ok(await _productService.GetAllAsync(categoryId, userCompanyId));
    }

    // GET api/<ProductController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<FullProductGet>> Get(Guid id)
    {
        var userCompanyIdString = HttpContext.Items["CompanyId"]?.ToString();

        return userCompanyIdString == null || !Guid.TryParse(userCompanyIdString, out Guid userCompanyId)
            ? (ActionResult<FullProductGet>)Forbid()
            : (ActionResult<FullProductGet>)(id == Guid.Empty
            ? throw new BadRequestException($"Null or Empty argument {nameof(id)}")
            : Ok(await _productService.GetByIdAsync(id, userCompanyId)));
    }

    // POST api/<ProductController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ProductPost request)
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

        await _productService.AddAsync(request, userCompanyId);
        return Ok();
    }

    // PUT api/<ProductController>/5
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] ProductPut request)
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

        await _productService.UpdateAsync(request, userCompanyId);
        return Ok();
    }

    // DELETE api/<ProductController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
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

        await _productService.DeleteAsync(id, userCompanyId);
        return Ok();
    }
}
