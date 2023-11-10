using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers
{
    public class ClaimsController : BaseController
    {

        private readonly ILogger<ClaimsController> _logger;
        private readonly IClaimsService _claimsService;

        public ClaimsController(ILogger<ClaimsController> logger, IClaimsService claimsService)
        {
            _logger = logger;
            _claimsService = claimsService;
        }

        /// <summary>
        /// Creates a Claim
        /// </summary>
        /// <response code="200">Retuns a sucess response with OK http status code</response>
        /// <response code="400">Returns bad request if related claim is missing or is not found and if the request doesn't comply with business logic</response>
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Claim claim)
        {
            return HandleResult(await _claimsService.CreateClaimAsync(claim));
        }

        /// <summary>
        /// Deletes a Claim
        /// </summary>
        /// <response code="200">Returns a sucess response with OK http status code</response>
        /// <response code="400">Returns bad request if request is not successful and provides the message returned from the external service</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            return HandleResult(await _claimsService.DeleteClaimByIdAsync(id));
        }

        /// <summary>
        /// Get a Claim by ClaimId
        /// </summary>
        /// <returns>The Claim that is found</returns>
        /// <response code="200">Returns a sucess response with OK http status code</response>
        /// <response code="404">Returns not found HTTP Response code if Claim is not found</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            return HandleResult(await _claimsService.GetClaimByIdAsync(id));
        }

        /// <summary>
        /// Get all Claims
        /// </summary>
        /// <returns>All claims found in the database</returns>
        /// <response code="200">Retuns a sucess response with OK http status code</response>
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return HandleResult(await _claimsService.GetAllClaimsAsync());
        }
    }
}