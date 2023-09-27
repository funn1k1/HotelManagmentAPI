using Microsoft.AspNetCore.Mvc;

namespace HotelManagment_API.Controllers.v2
{
    [ResponseCache(CacheProfileName = "Cache2Min")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    public class HotelAPIController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetHotelsAsync()
        {
            var hotels = new List<string> { "Hotel 1", "Hotel 2" };
            return Ok(hotels);
        }
    }
}