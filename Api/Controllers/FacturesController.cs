using Bussines.Dto.Factures;
using Bussines.Filters.FactureFilters.Models;
using Bussines.Services.Interfaces;
using Interface.FiltersInterface.PagingFilterInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FacturesController : ControllerBase
{
    private readonly IFactureService _factureService;

    public FacturesController(IFactureService factureService)
    {
        ArgumentNullException.ThrowIfNull(factureService);
        _factureService = factureService;
    }

    // GET: api/<FacturesController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BriefFacturesGet>>> Get(
        [FromQuery] FactureFilterCriteria factureSearchModel,
        [FromQuery] FactureSortOptions factureSortOptions,
        [FromQuery] PagingDtoOption pagingDtoOption)
    {
        var userCompanyIdString = HttpContext.Items["CompanyId"]?.ToString();

        return userCompanyIdString == null || !Guid.TryParse(userCompanyIdString, out Guid userCompanyId)
            ? (ActionResult<IEnumerable<BriefFacturesGet>>)Forbid()
            : (ActionResult<IEnumerable<BriefFacturesGet>>)Ok(await _factureService.GetFacturesAsync(factureSearchModel, factureSortOptions, pagingDtoOption, userCompanyId));
    }

    // GET api/<FacturesController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<FullFactureGet>> Get(Guid id)
    {
        var userCompanyIdString = HttpContext.Items["CompanyId"]?.ToString();

        return userCompanyIdString == null || !Guid.TryParse(userCompanyIdString, out Guid userCompanyId)
            ? (ActionResult<FullFactureGet>)Forbid()
            : (ActionResult<FullFactureGet>)Ok(await _factureService.GetFactureByIdAsync(id, userCompanyId));
    }

    // POST api/<FacturesController>
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] FacturePost value)
    {
        var userCompanyIdString = HttpContext.Items["CompanyId"]?.ToString();

        if (userCompanyIdString == null || !Guid.TryParse(userCompanyIdString, out Guid userCompanyId))
        {
            return Forbid();
        }

        await _factureService.AddAsync(value, userCompanyId);
        return Ok();
    }

    // PUT api/<FacturesController>/5
    [HttpPut]
    public async Task<ActionResult> Put([FromBody] FacturePut value)
    {
        var userCompanyIdString = HttpContext.Items["CompanyId"]?.ToString();

        if (userCompanyIdString == null || !Guid.TryParse(userCompanyIdString, out Guid userCompanyId))
        {
            return Forbid();
        }

        await _factureService.UpdateAsync(value, userCompanyId);
        return Ok();
    }

    // DELETE api/<FacturesController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var userCompanyIdString = HttpContext.Items["CompanyId"]?.ToString();

        if (userCompanyIdString == null || !Guid.TryParse(userCompanyIdString, out Guid userCompanyId))
        {
            return Forbid();
        }

        await _factureService.DeleteAsync(id, userCompanyId);
        return Ok();
    }
}
