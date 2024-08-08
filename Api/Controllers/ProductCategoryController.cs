using Bussines.Dto.ProductCategory;
using Bussines.Services.Interfaces;
using Interface.ExceptionsHandling.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[Route("api/[controller]")]
[ApiController]
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
    public async Task<IEnumerable<BriefProductCategoryGet>> Get()
    {
        return await _productCategoryService.GetAllAsync();
    }

    // GET api/<ProductCategoryController>/5
    [HttpGet("{id}")]
    public async Task<FullProductCategoryGet> Get(Guid id)
    {
        return id == Guid.Empty
            ? throw new BadRequestException($"Null or Empty argument {nameof(id)}")
            : await _productCategoryService.GetByIdAsync(id);
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

        await _productCategoryService.AddAsync(request);
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

        await _productCategoryService.UpdateAsync(request);
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

        await _productCategoryService.DeleteAsync(id);
        return Ok();
    }
}
