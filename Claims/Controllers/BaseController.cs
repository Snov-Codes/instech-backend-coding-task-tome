using Application.Core;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Claims.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if(result.StatusCode == HttpStatusCode.NotFound)
                return NotFound();

            if (result.IsSuccess && result.Value != null)
                return Ok(result.Value);

            if (result.IsSuccess)
                return Ok();

            return BadRequest(result.Error);
        }
    }
}
