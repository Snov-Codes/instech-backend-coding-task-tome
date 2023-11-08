using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers;
public class CoversController : BaseController
{
    private readonly ILogger<CoversController> _logger;
    private readonly ICoversService _coversService;

    public CoversController(ILogger<CoversController> logger, ICoversService coversService)
    {
        _logger = logger;
        _coversService = coversService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(Cover cover)
    {
        return HandleResult(await _coversService.CreateCoverAsync(cover));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        return HandleResult(await _coversService.DeleteCoverByIdAsync(id));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(string id)
    {
        return HandleResult(await _coversService.GetCoverByIdAsync(id));
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        return HandleResult(await _coversService.GetAllCoversAsync());
    }

    [HttpPost("{startDate}/{endDate}/{coverType}")]
    public ActionResult ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType)
    {
        return Ok(_coversService.ComputePremium(startDate, endDate, coverType));
    }
}