using HotelManagmentAPI.Data;
using HotelManagmentAPI.Logging.Enums;
using HotelManagmentAPI.Logging.Interfaces;
using HotelManagmentAPI.Models.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelAPIController : ControllerBase
    {
        private readonly ILogger<HotelAPIController> _logger;
        private readonly ILogging _logging;

        public HotelAPIController(ILogger<HotelAPIController> logger, ILogging logging)
        {
            _logger = logger;
            _logging = logging;
        }

        [HttpGet]
        public ActionResult<IEnumerable<HotelDTO>> GetHotels()
        {
            //_logger.LogInformation("Getting all hotels");
            _logging.Log(LogLevels.Error, "Getting all hotels");
            var hotels = HotelStore.Hotels;
            return Ok(hotels);
        }

        [HttpGet("{id}", Name = "GetHotel")]
        [
            ProducesResponseType(StatusCodes.Status404NotFound),
            ProducesResponseType(StatusCodes.Status200OK)
        ]
        public ActionResult<HotelDTO> GetHotel(int id)
        {
            var hotel = HotelStore.Hotels.Find(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return Ok(hotel);
        }

        [HttpPost]
        [
            ProducesResponseType(StatusCodes.Status200OK),
            ProducesResponseType(StatusCodes.Status201Created),
            ProducesResponseType(StatusCodes.Status400BadRequest),
            ProducesResponseType(StatusCodes.Status500InternalServerError)
        ]
        public ActionResult<HotelDTO> CreateHotel([FromBody] HotelDTO hotelDto)
        {
            var hotel = HotelStore.Hotels.Find(h => h.Name == hotelDto.Name);
            if (hotel != null)
            {
                ModelState.AddModelError("Name", "A hotel with this name already exists");
                return BadRequest(ModelState);
            }

            if (hotelDto.Id < 0)
            {
                return BadRequest("Id must be a positive integer");
            }

            hotelDto.Id = HotelStore.Hotels.Max(x => x.Id) + 1;
            HotelStore.Hotels.Add(hotelDto);
            return CreatedAtRoute("GetHotel", new { id = hotelDto.Id }, hotelDto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHotel(int id)
        {
            var hotel = HotelStore.Hotels.Find(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            HotelStore.Hotels.Remove(hotel);
            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHotel(int id, [FromBody] HotelDTO hotelDto)
        {
            if (id != hotelDto.Id)
            {
                return BadRequest("Check id match");
            }

            var hotel = HotelStore.Hotels.Find(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            hotel.Name = hotelDto.Name;
            hotel.Address = hotelDto.Address;

            return NoContent();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHotel(int id, [FromBody] JsonPatchDocument<HotelDTO> patchDto)
        {
            var hotel = HotelStore.Hotels.Find(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            patchDto.ApplyTo(hotel);

            return NoContent();
        }
    }
}
