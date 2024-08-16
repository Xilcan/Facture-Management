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
    public async Task<IEnumerable<BriefProductGet>> GetAll([FromQuery] Guid? categoryId)
    {
        return await _productService.GetAllAsync(categoryId);
    }

    // GET api/<ProductController>/5
    [HttpGet("{id}")]
    public async Task<FullProductGet> Get(Guid id)
    {
        return id == Guid.Empty
            ? throw new BadRequestException($"Null or Empty argument {nameof(id)}")
            : await _productService.GetByIdAsync(id);
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

        await _productService.AddAsync(request);
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

        await _productService.UpdateAsync(request);
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

        await _productService.DeleteAsync(id);
        return Ok();
    }
}
