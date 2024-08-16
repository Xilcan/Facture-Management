using Bussines.Dto.Company;
using Bussines.Services.Interfaces;
using Interface.ExceptionsHandling.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService)
    {
        ArgumentNullException.ThrowIfNull(companyService);
        _companyService = companyService;
    }

    // GET: api/<CompanyService>
    [HttpGet]
    public async Task<IEnumerable<BriefCompanyGet>> Get()
    {
        return await _companyService.GetAllAsync();
    }

    // GET api/<CompanyService>/5
    [HttpGet("{id}")]
    public async Task<FullCompanyGet> Get(Guid id)
    {
        return id == Guid.Empty
            ? throw new BadRequestException($"Null or Empty argument {nameof(id)}")
            : await _companyService.GetByIdAsync(id);
    }

    // POST api/<CompanyService>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CompanyPost request)
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

        await _companyService.AddAsync(request);
        return Ok();
    }

    // PUT api/<CompanyService>/5
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] FullCompanyPut request)
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

        await _companyService.UpdateAsync(request);
        return Ok();
    }

    // DELETE api/<CompanyService>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new BadRequestException($"Null or Empty argument {nameof(id)}");
        }

        await _companyService.DeleteAsync(id);
        return Ok();
    }
}
