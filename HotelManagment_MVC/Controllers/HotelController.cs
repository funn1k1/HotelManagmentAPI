using HotelManagment_MVC.Models;
using HotelManagment_MVC.Models.DTO.Hotel;
using HotelManagment_MVC.Services.Interfaces;
using HotelManagment_MVC.ViewModels.Hotel;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagment_MVC.Controllers
{
    public class HotelController : Controller
    {
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        public async Task<IActionResult> Index()
        {
            var getHotelsResp = await _hotelService.GetAllAsync<List<HotelDTO>>();
            if (!getHotelsResp.IsSuccess)
            {
                return View(new HotelViewModel { Hotels = new List<HotelDTO>() });
            }

            var hotelCreateVM = new HotelViewModel
            {
                Hotels = getHotelsResp.Result
            };
            return View(hotelCreateVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(HotelCreateViewModel hotelCreateVM)
        {
            if (ModelState.IsValid)
            {
                return View(hotelCreateVM);
            }

            var createHotelResp = await _hotelService.CreateAsync<List<Hotel>, HotelCreateDTO>(hotelCreateVM.Hotel);
            if (!createHotelResp.IsSuccess)
            {
                AddModelErrors(createHotelResp.ErrorMessages);
                return View(hotelCreateVM);
            }

            TempData["Success"] = "The hotel has been successfully created";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var getHotelResp = await _hotelService.GetAsync<HotelUpdateDTO, int>(id);
            if (!getHotelResp.IsSuccess)
            {
                AddModelErrors(getHotelResp.ErrorMessages);
                return View(new HotelUpdateViewModel { Hotel = new HotelUpdateDTO() });
            }

            var hotelUpdateVM = new HotelUpdateViewModel
            {
                Hotel = getHotelResp.Result
            };
            return View(hotelUpdateVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HotelUpdateViewModel hotelUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(hotelUpdateVM);
            }

            var updateHotelResp = await _hotelService.UpdateAsync<HotelDTO, HotelUpdateDTO>(hotelUpdateVM.Hotel);
            if (!updateHotelResp.IsSuccess)
            {
                AddModelErrors(updateHotelResp.ErrorMessages);
                return View(hotelUpdateVM);
            }

            TempData["Success"] = "The hotel has been successfully updated";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var getHotelResp = await _hotelService.GetAsync<HotelDTO, int>(id);
            if (!getHotelResp.IsSuccess)
            {
                AddModelErrors(getHotelResp.ErrorMessages);
                return View(new HotelDeleteViewModel { Hotel = new HotelDTO() });
            }

            return View(getHotelResp.Result);
        }

        [HttpPost]
        public async Task<IActionResult> PostDelete(int id)
        {
            var deleteHotelResp = await _hotelService.DeleteAsync<HotelDTO, int>(id);
            if (!deleteHotelResp.IsSuccess)
            {
                AddModelErrors(deleteHotelResp.ErrorMessages);
                return View("Delete", new HotelDeleteViewModel { Hotel = new HotelDTO() });
            }

            TempData["Success"] = "The hotel has been successfully deleted";
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
