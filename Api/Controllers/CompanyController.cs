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
    public async Task<ActionResult<IEnumerable<BriefCompanyGet>>> Get()
    {
        var userCompanyIdString = HttpContext.Items["CompanyId"]?.ToString();

        return userCompanyIdString == null || !Guid.TryParse(userCompanyIdString, out Guid userCompanyId)
            ? (ActionResult<IEnumerable<BriefCompanyGet>>)Forbid()
            : (ActionResult<IEnumerable<BriefCompanyGet>>)Ok(await _companyService.GetAllAsync(userCompanyId));
    }

    // GET api/<CompanyService>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<FullCompanyGet>> Get(Guid id)
    {
        var userCompanyIdString = HttpContext.Items["CompanyId"]?.ToString();

        return userCompanyIdString == null || !Guid.TryParse(userCompanyIdString, out Guid userCompanyId)
            ? (ActionResult<FullCompanyGet>)Forbid()
            : (ActionResult<FullCompanyGet>)(id == Guid.Empty
            ? throw new BadRequestException($"Null or Empty argument {nameof(id)}")
            : Ok(await _companyService.GetByIdAsync(id, userCompanyId)));
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

        var userCompanyIdString = HttpContext.Items["CompanyId"]?.ToString();

        if (userCompanyIdString == null || !Guid.TryParse(userCompanyIdString, out Guid userCompanyId))
        {
            return Forbid();
        }

        await _companyService.AddAsync(request, userCompanyId);
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

        var userCompanyIdString = HttpContext.Items["CompanyId"]?.ToString();

        if (userCompanyIdString == null || !Guid.TryParse(userCompanyIdString, out Guid userCompanyId))
        {
            return Forbid();
        }

        await _companyService.UpdateAsync(request, userCompanyId);
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

        var userCompanyIdString = HttpContext.Items["CompanyId"]?.ToString();

        if (userCompanyIdString == null || !Guid.TryParse(userCompanyIdString, out Guid userCompanyId))
        {
            return Forbid();
        }

        await _companyService.DeleteAsync(id, userCompanyId);
        return Ok();
    }
}
