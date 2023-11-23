using HotelManagment_MVC.Models;
using HotelManagment_MVC.Models.DTO.Hotel;
using HotelManagment_MVC.Models.DTO.Room;
using HotelManagment_MVC.Services.Interfaces;
using HotelManagment_MVC.ViewModels.Room;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelManagment_MVC.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IHotelService _hotelService;
        private readonly ITokenProvider _tokenProvider;

        public RoomController(IRoomService roomService, IHotelService hotelService, ITokenProvider tokenProvider)
        {
            _roomService = roomService;
            _hotelService = hotelService;
            _tokenProvider = tokenProvider;
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomCreateViewModel roomCreateVM)
        {
            if (!ModelState.IsValid)
            {
                var getHotelsResp = await _hotelService.GetAllAsync<List<HotelDTO>>();
                if (!getHotelsResp.IsSuccess)
                {
                    AddModelErrors(getHotelsResp.ErrorMessages);
                    return View(roomCreateVM);
                }

                roomCreateVM.Hotels = getHotelsResp.Result.Select(h => new SelectListItem
                {
                    Text = h.Name,
                    Value = h.Id.ToString()
                });
                return View(roomCreateVM);
            }

            var createRoomResp = await _roomService.CreateAsync<Room, RoomCreateDTO>(roomCreateVM.Room);
            if (!createRoomResp.IsSuccess)
            {
                AddModelErrors(createRoomResp.ErrorMessages);
                return View(roomCreateVM);
            }

            TempData["Success"] = "The room has been successfully established";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            var getRoomResp = await _roomService.GetAsync<RoomUpdateDTO>(id);
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(RoomUpdateViewModel roomUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(roomUpdateVM);
            }

            var updateRoomResp = await _roomService.UpdateAsync<RoomDTO, RoomUpdateDTO>(roomUpdateVM.Room);
            if (!updateRoomResp.IsSuccess)
            {
                AddModelErrors(updateRoomResp.ErrorMessages);
                return View(roomUpdateVM);
            }

            TempData["Success"] = "The room has been successfully updated";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var getRoomResp = await _roomService.GetAsync<RoomDTO>(id);
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostDelete(int id)
        {
            var deleteRoomResp = await _roomService.DeleteAsync<RoomDTO>(id);
            if (!deleteRoomResp.IsSuccess)
            {
                AddModelErrors(deleteRoomResp.ErrorMessages);
                return View("Delete", new RoomDeleteViewModel { Room = new RoomDTO() });
            }

            TempData["Success"] = "The room has been successfully deleted";
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
