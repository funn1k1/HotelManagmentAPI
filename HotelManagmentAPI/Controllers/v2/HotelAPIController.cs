using Microsoft.AspNetCore.Mvc;

namespace HotelManagment_API.Controllers.v2
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    public class HotelAPIController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetHotelsAsync()
        {
            return Ok(new List<string> { "Hotel 1", "Hotel 2" });
        }
    }
}