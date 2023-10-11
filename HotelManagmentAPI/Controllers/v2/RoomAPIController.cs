using System.Net;
using AutoMapper;
using HotelManagment_API.Models;
using HotelManagment_API.Models.DTO.Room;
using HotelManagment_API.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagment_API.Controllers.v2
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    public class RoomAPIController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRoomRepository _roomRepo;
        private readonly IHotelRepository _hotelRepo;

        public RoomAPIController(
            IRoomRepository roomRepo,
            IHotelRepository hotelRepo,
            IMapper mapper
        )
        {
            _mapper = mapper;
            _roomRepo = roomRepo;
            _hotelRepo = hotelRepo;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse<List<RoomDTO>>>> GetRoomsAsync([Bind()] int pageNumber, int pageSize)
        {
            var response = new APIResponse<List<RoomDTO>>
            {
                IsSuccess = true,
                Result = _mapper.Map<List<RoomDTO>>(
                    await _roomRepo.GetAllAsync(includeProperties: r => r.Hotel, isTracked: false, pageNumber: pageNumber, pageSize: pageSize)
                ),
                StatusCode = HttpStatusCode.OK
            };
            return Ok(response);
        }

        [HttpGet("{id:int}", Name = "GetRoom")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse<RoomDTO>>> GetRoomAsync(int id)
        {
            var response = new APIResponse<RoomDTO>();
            var room = await _roomRepo.GetAsync(
                r => r.Id == id,
                includeProperties: r => r.Hotel
            );
            if (room == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.AddErrorMessage("Room not found");
                return NotFound(response);
            }

            response.Result = _mapper.Map<RoomDTO>(room);
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Ok(response);
        }

        [ResponseCache(CacheProfileName = "Cache2Min")]

        [HttpGet("{roomNumber}/hotels/{hotelId}", Name = "GetRoomByNumber")]
        [
            ProducesResponseType(StatusCodes.Status200OK),
            ProducesResponseType(StatusCodes.Status404NotFound)
        ]
        public async Task<ActionResult<APIResponse<RoomDTO>>> GetRoomAsync(string roomNumber, int hotelId)
        {
            var response = new APIResponse<RoomDTO>();
            var room = await _roomRepo.GetAsync(
                r => r.RoomNumber.ToLower() == roomNumber.ToLower() &&
                r.HotelId == hotelId,
                includeProperties: r => r.Hotel
            );
            if (room == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.AddErrorMessage("Room not found");
                return NotFound(response);
            }

            response.Result = _mapper.Map<RoomDTO>(room);
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateRoomAsync([FromBody] RoomCreateDTO roomDto)
        {
            var response = new APIResponse<Room>();
            var hotel = await _hotelRepo.GetAsync(h => h.Id == roomDto.HotelId);
            if (hotel == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.AddErrorMessage("Hotel with this id does not exist");
                return BadRequest(response);
            }

            var room = await _roomRepo.GetAsync(r => r.RoomNumber == roomDto.RoomNumber && hotel.Id == r.Hotel.Id);
            if (room != null)
            {
                //ModelState.AddModelError("Name", "A hotel with this name already exists");
                response.StatusCode = HttpStatusCode.BadRequest;
                response.AddErrorMessage("This room number already exists");
                return BadRequest(response);
            }

            var newRoom = _mapper.Map<Room>(roomDto);
            newRoom.CreatedDate = DateTime.Now;
            await _roomRepo.AddAsync(newRoom);

            response.Result = newRoom;
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.Created;
            return CreatedAtRoute("GetRoom", new { id = response.Result.Id }, response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRoomAsync(int id)
        {
            var response = new APIResponse<RoomDTO>();
            var room = await _roomRepo.GetAsync(r => r.Id == id);
            if (room == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.AddErrorMessage("Room not found");
                return NotFound(response);
            }

            await _roomRepo.DeleteAsync(room);

            response.Result = _mapper.Map<RoomDTO>(room);
            response.StatusCode = HttpStatusCode.NoContent;
            response.IsSuccess = true;
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRoomAsync(int id, [FromBody] RoomUpdateDTO roomDto)
        {
            var response = new APIResponse<RoomDTO>();
            var room = await _roomRepo.GetAsync(
                r => r.Id == id &&
                r.HotelId == roomDto.HotelId
            );
            if (room == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.AddErrorMessage("Room not found");
                return NotFound(response);
            }

            _mapper.Map(roomDto, room);
            await _roomRepo.UpdateAsync(room);

            response.Result = _mapper.Map<RoomDTO>(room);
            response.StatusCode = HttpStatusCode.NoContent;
            response.IsSuccess = true;
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRoomAsync(int id, [FromBody] JsonPatchDocument<RoomUpdateDTO> patchDto)
        {
            var response = new APIResponse<RoomDTO>();
            var room = await _roomRepo.GetAsync(r => r.Id == id);
            if (room == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.AddErrorMessage("Room not found");
                return NotFound(response);
            }

            var roomUpdateDto = _mapper.Map<RoomUpdateDTO>(room);
            patchDto.ApplyTo(roomUpdateDto);
            _mapper.Map(roomUpdateDto, room);
            await _roomRepo.UpdateAsync(room);

            response.Result = _mapper.Map<RoomDTO>(room);
            response.StatusCode = HttpStatusCode.NoContent;
            response.IsSuccess = true;
            return Ok(response);
        }
    }
}