using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class CoversController : ControllerBase
{
    private readonly ILogger<CoversController> _logger;
    private readonly ICoversService _coversService;

    public CoversController(ILogger<CoversController> logger, ICoversService coversService)
    {
        _logger = logger;
        _coversService = coversService;
    }

    [HttpPost]
    public async Task CreateAsync(Cover cover)
    {
        await _coversService.CreateCoverAsync(cover);
        //await _auditer.AuditCover(cover.Id, "POST");
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(string id)
    {
        await _coversService.DeleteCoverByIdAsync(id);
        //await _auditer.AuditCover(cover.Id, "DELETE");
    }

    [HttpGet("{id}")]
    public async Task<Cover> GetAsync(string id)
    {
        return await _coversService.GetCoverByIdAsync(id);
    }

    [HttpGet]
    public async Task<IEnumerable<Cover>> GetAsync()
    {
        return await _coversService.GetAllCoversAsync();
    }
    //[HttpPost("{startDate}/{endDate}/{coverType}")]
    //public async Task<ActionResult> ComputePremiumAsync(DateOnly startDate, DateOnly endDate, CoverType coverType)
    //{
    //    return Ok(ComputePremium(startDate, endDate, coverType));
    //}
}