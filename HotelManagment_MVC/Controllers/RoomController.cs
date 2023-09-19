using HotelManagment_MVC.Models;
using HotelManagment_MVC.Models.DTO.Hotel;
using HotelManagment_MVC.Models.DTO.Room;
using HotelManagment_MVC.Services.Interfaces;
using HotelManagment_MVC.ViewModels.Room;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelManagment_MVC.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IHotelService _hotelService;

        public RoomController(IRoomService roomService, IHotelService hotelService)
        {
            _roomService = roomService;
            _hotelService = hotelService;
        }

        public async Task<IActionResult> Index()
        {
            var getRoomsResp = await _roomService.GetAllAsync<List<RoomDTO>>();
            if (!getRoomsResp.IsSuccess)
            {
                return View(new RoomViewModel { Rooms = new List<RoomDTO>() });
            }

            var roomVM = new RoomViewModel()
            {
                Rooms = getRoomsResp.Result
            };
            return View(roomVM);
        }

        public async Task<IActionResult> Create()
        {
            var getHotelsResp = await _hotelService.GetAllAsync<List<HotelDTO>>();
            if (!getHotelsResp.IsSuccess)
            {
                AddModelErrors(getHotelsResp.ErrorMessages);
                return View(new RoomCreateViewModel { Room = new RoomCreateDTO() });
            }

            var roomCreateVM = new RoomCreateViewModel
            {
                Hotels = getHotelsResp.Result.Select(h => new SelectListItem
                {
                    Text = h.Name,
                    Value = h.Id.ToString()
                }),
                Room = new RoomCreateDTO()
            };
            return View(roomCreateVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoomCreateViewModel roomCreateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(roomCreateVM);
            }

            var getRoomsResp = await _hotelService.GetAllAsync<List<HotelDTO>>();
            if (!getRoomsResp.IsSuccess)
            {
                AddModelErrors(getRoomsResp.ErrorMessages);
                return View(roomCreateVM);
            }

            if (!ModelState.IsValid)
            {
                roomCreateVM.Hotels = getRoomsResp.Result.Select(h => new SelectListItem
                {
                    Text = h.Name,
                    Value = h.Id.ToString()
                });

                return View(roomCreateVM);
            }

            var createRoomResp = await _roomService.CreateAsync<List<Room>, RoomCreateDTO>(roomCreateVM.Room);
            if (!createRoomResp.IsSuccess)
            {
                roomCreateVM.Hotels = getRoomsResp.Result.Select(h => new SelectListItem
                {
                    Text = h.Name,
                    Value = h.Id.ToString()
                });
                AddModelErrors(createRoomResp.ErrorMessages);
                return View(roomCreateVM);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var getRoomResp = await _roomService.GetAsync<RoomUpdateDTO, int>(id);
            if (!getRoomResp.IsSuccess)
            {
                AddModelErrors(getRoomResp.ErrorMessages);
                return View(new RoomUpdateViewModel { Room = new RoomUpdateDTO() });
            }

            var updateRoomVM = new RoomUpdateViewModel
            {
                Room = getRoomResp.Result
            };
            return View(updateRoomVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(RoomUpdateDTO roomUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(roomUpdateDto);
            }

            var updateRoomResp = await _roomService.UpdateAsync<RoomDTO, RoomUpdateDTO>(roomUpdateDto);
            if (!updateRoomResp.IsSuccess)
            {
                AddModelErrors(updateRoomResp.ErrorMessages);
                return View(roomUpdateDto);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var getRoomResp = await _roomService.GetAsync<RoomDTO, int>(id);
            if (!getRoomResp.IsSuccess)
            {
                AddModelErrors(getRoomResp.ErrorMessages);
                return View(new RoomDeleteViewModel { Room = new RoomDTO() });
            }

            var roomDeleteVM = new RoomDeleteViewModel
            {
                Room = getRoomResp.Result
            };
            return View(roomDeleteVM);
        }

        [HttpPost]
        public async Task<IActionResult> PostDelete(int id)
        {
            var deleteRoomResp = await _roomService.DeleteAsync<RoomDTO, int>(id);
            if (!deleteRoomResp.IsSuccess)
            {
                AddModelErrors(deleteRoomResp.ErrorMessages);
                return View("Delete", new RoomDeleteViewModel { Room = new RoomDTO() });
            }

            return RedirectToAction(nameof(Index));
        }

        private void AddModelErrors(IEnumerable<string> errorMessages)
        {
            foreach (var errorMessage in errorMessages)
            {
                ModelState.AddModelError(string.Empty, errorMessage);
            }
        }
    }
}
