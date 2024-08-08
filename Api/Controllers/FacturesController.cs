using Bussines.Dto.Factures;
using Bussines.Filters.FactureFilters.Models;
using Bussines.Services.Interfaces;
using Interface.FiltersInterface.PagingFilterInterface;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[Route("api/[controller]")]
[ApiController]
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
    public async Task<IEnumerable<BriefFacturesGet>> Get(
        [FromQuery] FactureFilterCriteria factureSearchModel,
        [FromQuery] FactureSortOptions factureSortOptions,
        [FromQuery] PagingDtoOption pagingDtoOption)
    {
        return await _factureService.GetFacturesAsync(factureSearchModel, factureSortOptions, pagingDtoOption);
    }

    // GET api/<FacturesController>/5
    [HttpGet("{id}")]
    public async Task<FullFactureGet> Get(Guid id)
    {
        return await _factureService.GetFactureByIdAsync(id);
    }

    // POST api/<FacturesController>
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] FacturePost value)
    {
        await _factureService.AddAsync(value);
        return Ok();
    }

    // PUT api/<FacturesController>/5
    [HttpPut]
    public async Task<ActionResult> Put([FromBody] FacturePut value)
    {
        await _factureService.UpdateAsync(value);
        return Ok();
    }

    // DELETE api/<FacturesController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _factureService.DeleteAsync(id);
        return Ok();
    }
}
