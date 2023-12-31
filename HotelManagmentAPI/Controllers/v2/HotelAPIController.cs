﻿using System.Net;
using Asp.Versioning;
using AutoMapper;
using HotelManagment_API.Models;
using HotelManagment_API.Models.DTO.Hotel;
using HotelManagment_API.Repository.Interfaces;
using HotelManagment_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HotelManagment_API.Controllers.v2
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    public class HotelAPIController : ControllerBase
    {
        private readonly IHotelRepository _hotelRepo;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public HotelAPIController(IHotelRepository hotelRepo, IImageService imageService, IMapper mapper)
        {
            _hotelRepo = hotelRepo;
            _imageService = imageService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse<List<HotelDTO>>>> GetHotelsAsync(int pageNumber, int pageSize)
        {
            try
            {
                var response = new APIResponse<List<HotelDTO>>
                {
                    IsSuccess = true,
                    Result = _mapper.Map<List<HotelDTO>>(
                        await _hotelRepo.GetAllAsync(includeProperties: h => h.Rooms, isTracked: false, pageNumber: pageNumber, pageSize: pageSize)
                    ),
                    StatusCode = HttpStatusCode.OK
                };

                var pagination = new
                {
                    pageNumber,
                    pageSize,
                };
                Response.Headers.Add("x-pagination", JsonConvert.SerializeObject(pagination));
                return Ok(response);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while getting hotels");
            }
        }

        [HttpGet("{id:int}", Name = "GetHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse<HotelDTO>>> GetHotelAsync(int id)
        {
            try
            {
                var response = new APIResponse<HotelDTO>();
                var hotel = await _hotelRepo.GetAsync(h => h.Id == id, includeProperties: h => h.Rooms);
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
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while getting the hotel");
            }
        }

        [ResponseCache(CacheProfileName = "Cache2Min")]
        [HttpGet("{name}", Name = "GetHotelByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse<HotelDTO>>> GetHotelByNameAsync(string name)
        {
            try
            {
                var response = new APIResponse<HotelDTO>();
                var hotel = await _hotelRepo.GetAsync(h => h.Name.ToLower() == name.ToLower(), includeProperties: h => h.Rooms);
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
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while getting the hotel by name");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHotelAsync([FromForm] HotelCreateDTO hotelDto)
        {
            try
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

                hotelDto.ImageUrl = await _imageService.UploadAsync(hotelDto.ImageFile) ?? hotelDto.ImageUrl;
                var newHotel = _mapper.Map<Hotel>(hotelDto);
                newHotel.CreatedDate = DateTime.Now;

                await _hotelRepo.AddAsync(newHotel);

                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.Created;
                response.Result = newHotel;
                return CreatedAtRoute("GetHotel", new { id = response.Result.Id }, response);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while creating the hotel");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteHotelAsync(int id)
        {
            try
            {
                var response = new APIResponse<HotelDTO>();
                var hotel = await _hotelRepo.GetAsync(h => h.Id == id);
                if (hotel == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.AddErrorMessage("Hotel not found");
                    return NotFound(response);
                }

                if (!string.IsNullOrEmpty(hotel.ImageUrl))
                {
                    _imageService.Delete(hotel.ImageUrl);
                }
                await _hotelRepo.DeleteAsync(hotel);

                response.Result = _mapper.Map<HotelDTO>(hotel);
                response.StatusCode = HttpStatusCode.OK;
                response.IsSuccess = true;
                return Ok(response);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while deleting the hotel");
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotelAsync(int id, [FromForm] HotelUpdateDTO hotelDto)
        {
            try
            {
                var response = new APIResponse<HotelDTO>();
                var hotel = await _hotelRepo.GetAsync(h => h.Id == id, includeProperties: h => h.Rooms);
                if (hotel == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.AddErrorMessage("Hotel not found");
                    return NotFound(response);
                }

                hotelDto.ImageUrl = await _imageService.UpdateAsync(hotelDto.ImageFile, hotel.ImageUrl) ?? hotelDto.ImageUrl;
                _mapper.Map(hotelDto, hotel);
                await _hotelRepo.UpdateAsync(hotel);

                response.Result = _mapper.Map<HotelDTO>(hotel);
                response.StatusCode = HttpStatusCode.OK;
                response.IsSuccess = true;
                return Ok(response);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while updating the hotel");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotelAsync(int id, [FromBody] JsonPatchDocument<HotelUpdateDTO> patchDto)
        {
            try
            {
                var response = new APIResponse<HotelDTO>();
                var hotel = await _hotelRepo.GetAsync(h => h.Id == id, includeProperties: h => h.Rooms);
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

                response.Result = _mapper.Map<HotelDTO>(hotel);
                response.StatusCode = HttpStatusCode.OK;
                response.IsSuccess = true;
                return Ok(response);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured during a partial update of the hotel");
            }
        }
    }
}