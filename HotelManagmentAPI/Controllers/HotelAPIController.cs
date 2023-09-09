using AutoMapper;
using HotelManagmentAPI.Data;
using HotelManagmentAPI.Models;
using HotelManagmentAPI.Models.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelManagmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public HotelAPIController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelDTO>>> GetHotelsAsync()
        {
            var hotels = await _db.Hotels.ToListAsync();
            return Ok(_mapper.Map<List<HotelDTO>>(hotels));
        }

        [HttpGet("{id}", Name = "GetHotel")]
        [
            ProducesResponseType(StatusCodes.Status404NotFound),
            ProducesResponseType(StatusCodes.Status200OK)
        ]
        public async Task<ActionResult<HotelDTO>> GetHotelAsync(int id)
        {
            var hotel = await _db.Hotels.FirstOrDefaultAsync(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<HotelDTO>(hotel));
        }

        [HttpPost]
        [
            ProducesResponseType(StatusCodes.Status200OK),
            ProducesResponseType(StatusCodes.Status201Created),
            ProducesResponseType(StatusCodes.Status400BadRequest),
            ProducesResponseType(StatusCodes.Status500InternalServerError)
        ]
        public async Task<ActionResult<HotelDTO>> CreateHotelAsync([FromBody] HotelCreateDTO hotelDto)
        {
            var hotel = await _db.Hotels.FirstOrDefaultAsync(h => h.Name == hotelDto.Name);
            if (hotel != null)
            {
                ModelState.AddModelError("Name", "A hotel with this name already exists");
                return BadRequest(ModelState);
            }

            var newHotel = _mapper.Map<Hotel>(hotelDto);

            await _db.Hotels.AddAsync(newHotel);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetHotel", new { id = newHotel.Id }, _mapper.Map<HotelDTO>(newHotel));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteHotelAsync(int id)
        {
            var hotel = await _db.Hotels.FirstOrDefaultAsync(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            _db.Hotels.Remove(hotel);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateHotelAsync(int id, [FromBody] HotelUpdateDTO hotelDto)
        {
            var hotel = await _db.Hotels.FirstOrDefaultAsync(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            _mapper.Map(hotelDto, hotel);
            _db.Hotels.Update(hotel);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateHotelAsync(int id, [FromBody] JsonPatchDocument<HotelUpdateDTO> patchDto)
        {
            var hotel = await _db.Hotels.FirstOrDefaultAsync(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            var hotelUpdateDto = _mapper.Map<HotelUpdateDTO>(hotel);
            patchDto.ApplyTo(hotelUpdateDto);
            _mapper.Map(hotelUpdateDto, hotel);
            _db.Hotels.Update(hotel);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
