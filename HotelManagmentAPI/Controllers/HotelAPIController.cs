using System.Net;
using AutoMapper;
using HotelManagmentAPI.Models;
using HotelManagmentAPI.Models.DTO.Hotel;
using HotelManagmentAPI.Repository.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelAPIController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHotelRepository _hotelRepo;

        public HotelAPIController(IHotelRepository hotelRepo, IMapper mapper)
        {
            _mapper = mapper;
            _hotelRepo = hotelRepo;
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse<List<HotelDTO>>>> GetHotelsAsync()
        {
            var response = new APIResponse<List<HotelDTO>>
            {
                IsSuccess = true,
                Result = _mapper.Map<List<HotelDTO>>(await _hotelRepo.GetAllAsync()),
                StatusCode = HttpStatusCode.OK
            };
            return Ok(response);
        }

        [HttpGet("{id}", Name = "GetHotel")]
        [
            ProducesResponseType(StatusCodes.Status404NotFound),
            ProducesResponseType(StatusCodes.Status200OK)
        ]
        public async Task<ActionResult<HotelDTO>> GetHotelAsync(int id)
        {
            var response = new APIResponse<HotelDTO>();
            var hotel = await _hotelRepo.GetAsync(x => x.Id == id);
            if (hotel == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.AddErrorMessage("Hotel not found");
                return NotFound(response);
            }

            response.Result = _mapper.Map<HotelDTO>(hotel);

            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Ok(response);
        }

        [HttpPost]
        [
            ProducesResponseType(StatusCodes.Status200OK),
            ProducesResponseType(StatusCodes.Status201Created),
            ProducesResponseType(StatusCodes.Status400BadRequest),
            ProducesResponseType(StatusCodes.Status500InternalServerError)
        ]
        public async Task<IActionResult> CreateHotelAsync([FromBody] HotelCreateDTO hotelDto)
        {
            var response = new APIResponse<Hotel>();
            var hotel = await _hotelRepo.GetAsync(h => h.Name == hotelDto.Name);
            if (hotel != null)
            {
                //ModelState.AddModelError("Name", "A hotel with this name already exists");
                response.StatusCode = HttpStatusCode.BadRequest;
                response.AddErrorMessage("Hotel with this name already exists");
                return BadRequest(response);
            }

            var newHotel = _mapper.Map<Hotel>(hotelDto);
            await _hotelRepo.AddAsync(newHotel);

            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.Created;
            response.Result = newHotel;
            return CreatedAtRoute("GetHotel", new { id = response.Result.Id }, response.Result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteHotelAsync(int id)
        {
            var response = new APIResponse<HotelDTO>();
            var hotel = await _hotelRepo.GetAsync(h => h.Id == id);
            if (hotel == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.AddErrorMessage("Hotel not found");
                return NotFound(response);
            }

            await _hotelRepo.DeleteAsync(hotel);

            response.StatusCode = HttpStatusCode.NoContent;
            response.IsSuccess = true;
            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateHotelAsync(int id, [FromBody] HotelUpdateDTO hotelDto)
        {
            var response = new APIResponse<Hotel>();
            var hotel = await _hotelRepo.GetAsync(h => h.Id == id);
            if (hotel == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.AddErrorMessage("Hotel not found");
                return NotFound(response);
            }

            _mapper.Map(hotelDto, hotel);
            await _hotelRepo.UpdateAsync(hotel);

            response.StatusCode = HttpStatusCode.NoContent;
            response.IsSuccess = true;
            return NoContent();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateHotelAsync(int id, [FromBody] JsonPatchDocument<HotelUpdateDTO> patchDto)
        {
            var response = new APIResponse<Hotel>();
            var hotel = await _hotelRepo.GetAsync(h => h.Id == id);
            if (hotel == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.AddErrorMessage("Hotel not found");
                return NotFound(response);
            }

            var hotelUpdateDto = _mapper.Map<HotelUpdateDTO>(hotel);
            patchDto.ApplyTo(hotelUpdateDto);
            _mapper.Map(hotelUpdateDto, hotel);
            await _hotelRepo.UpdateAsync(hotel);

            response.StatusCode = HttpStatusCode.NoContent;
            response.IsSuccess = true;
            return NoContent();
        }
    }
}
