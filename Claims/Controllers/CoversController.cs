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

    /// <summary>
    /// Creates a Cover
    /// </summary>
    /// <response code="200">Returns a sucess response with OK http status code</response>
    /// <response code="400">Returns bad request if the request doesn't comply with business logic</response>
    [HttpPost]
    public async Task<IActionResult> CreateAsync(Cover cover)
    {
        return HandleResult(await _coversService.CreateCoverAsync(cover));
    }

    /// <summary>
    /// Deletes a Cover
    /// </summary>
    /// <response code="200">Returns a sucess response with OK http status code</response>
    /// <response code="400">Returns bad request if request is not successful and provides the message returned from the external service</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        return HandleResult(await _coversService.DeleteCoverByIdAsync(id));
    }

    /// <summary>
    /// Get a single Cover by CoverId
    /// </summary>
    /// <returns>The Cover that is found</returns>
    /// <response code="200">Returns a sucess response with OK http status code</response>
    /// <response code="404">Returns not found HTTP Response code if Claim is not found</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(string id)
    {
        return HandleResult(await _coversService.GetCoverByIdAsync(id));
    }

    /// <summary>
    /// Get all Covers
    /// </summary>
    /// <returns>All Covers found in the database</returns>
    /// <response code="200">Retuns a sucess response with OK http status code</response>
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        return HandleResult(await _coversService.GetAllCoversAsync());
    }

    /// <summary>
    /// Calculate cover premium
    /// </summary>
    /// <returns>Returns the decimal number that is an output of the premium calculation formula</returns>
    /// <response code="200">Retuns a sucess response with OK http status code</response>
    [HttpPost("{startDate}/{endDate}/{coverType}")]
    public ActionResult ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType)
    {
        return Ok(_coversService.ComputePremium(startDate, endDate, coverType));
    }
}