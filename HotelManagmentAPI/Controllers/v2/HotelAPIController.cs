using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HotelManagment_API.Controllers.v2
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    public class HotelAPIController : ControllerBase
    {
        private List<string> _hotels = new();

        public HotelAPIController()
        {
            for (int i = 1; i <= 10; i++)
            {
                _hotels.Add($"Hotel {i}");
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetHotelsAsync(int pageNumber = 1, int pageSize = 3)
        {
            var pagination = new
            {
                pageNumber,
                pageSize
            };
            Response.Headers.Add("x-pagination", JsonConvert.SerializeObject(pagination));
            var hotels = _hotels.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
            return Ok(hotels);
        }
    }
}