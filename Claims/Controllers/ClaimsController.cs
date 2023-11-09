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
        /// Creates a Claim.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>A newly created Claim</returns>
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Claim claim)
        {
            return HandleResult(await _claimsService.CreateClaimAsync(claim));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            return HandleResult(await _claimsService.DeleteClaimByIdAsync(id));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            return HandleResult(await _claimsService.GetClaimByIdAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return HandleResult(await _claimsService.GetAllClaimsAsync());
        }
    }
}