using HotelManagmentAPI.Data;
using HotelManagmentAPI.Models;
using HotelManagmentAPI.Models.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public HotelAPIController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<HotelDTO>> GetHotels()
        {
            var hotels = _db.Hotels.ToList();
            return Ok(hotels);
        }

        [HttpGet("{id}", Name = "GetHotel")]
        [
            ProducesResponseType(StatusCodes.Status404NotFound),
            ProducesResponseType(StatusCodes.Status200OK)
        ]
        public ActionResult<HotelDTO> GetHotel(int id)
        {
            var hotel = _db.Hotels.FirstOrDefault(h => h.Id == id);
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
            var hotel = _db.Hotels.FirstOrDefault(h => h.Name == hotelDto.Name);
            if (hotel != null)
            {
                ModelState.AddModelError("Name", "A hotel with this name already exists");
                return BadRequest(ModelState);
            }

            if (hotelDto.Id < 0 || hotelDto.Id > 0)
            {
                return BadRequest("Id must be 0");
            }

            hotelDto.Id = _db.Hotels.Max(x => x.Id) + 1;
            var newHotel = new Hotel
            {
                Name = hotelDto.Name,
                Address = hotelDto.Address,
                ImageUrl = hotelDto.ImageUrl,
                Description = hotelDto.Description,
                Rating = hotelDto.Rating,
                PhoneNumber = hotelDto.PhoneNumber,
                Email = hotelDto.Email,
                CreatedDate = DateTime.Now,
            };

            _db.Hotels.Add(newHotel);
            _db.SaveChanges();
            return CreatedAtRoute("GetHotel", new { id = newHotel.Id }, newHotel);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHotel(int id)
        {
            var hotel = _db.Hotels.FirstOrDefault(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            _db.Hotels.Remove(hotel);
            _db.SaveChanges();
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

            var hotel = _db.Hotels.FirstOrDefault(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            hotel.Name = hotelDto.Name;
            hotel.Address = hotelDto.Address;
            hotel.Description = hotelDto.Description;
            hotel.ImageUrl = hotelDto.ImageUrl;
            hotel.Rating = hotelDto.Rating;
            hotel.PhoneNumber = hotelDto.PhoneNumber;
            hotel.Email = hotelDto.Email;

            _db.Hotels.Update(hotel);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHotel(int id, [FromBody] JsonPatchDocument<HotelDTO> patchDto)
        {
            var hotel = _db.Hotels.FirstOrDefault(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            var hotelDto = new HotelDTO
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Address = hotel.Address,
                ImageUrl = hotel.ImageUrl,
                Description = hotel.Description,
                Rating = hotel.Rating,
                PhoneNumber = hotel.PhoneNumber,
                Email = hotel.Email,
            };
            patchDto.ApplyTo(hotelDto);

            hotel.Name = hotelDto.Name;
            hotel.Address = hotelDto.Address;
            hotel.ImageUrl = hotelDto.ImageUrl;
            hotel.Description = hotelDto.Description;
            hotel.Rating = hotelDto.Rating;
            hotel.PhoneNumber = hotelDto.PhoneNumber;
            hotel.Email = hotelDto.Email;

            _db.Hotels.Update(hotel);
            _db.SaveChanges();
            return NoContent();
        }
    }
}
