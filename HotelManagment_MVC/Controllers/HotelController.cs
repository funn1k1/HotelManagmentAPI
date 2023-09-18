using HotelManagment_MVC.Models;
using HotelManagment_MVC.Models.DTO.Hotel;
using HotelManagment_MVC.Services.Interfaces;
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
            var response = await _hotelService.GetAllAsync<List<HotelDTO>>();
            if (!response.IsSuccess)
            {
                return View(new List<HotelDTO>());
            }

            return View(response.Result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(HotelCreateDTO hotelCreateDto)
        {
            var response = await _hotelService.CreateAsync<List<Hotel>, HotelCreateDTO>(hotelCreateDto);
            if (!response.IsSuccess)
            {
                AddModelErrors(response.ErrorMessages);
                return View(hotelCreateDto);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var response = await _hotelService.GetAsync<HotelUpdateDTO, int>(id);
            if (!response.IsSuccess)
            {
                AddModelErrors(response.ErrorMessages);
                return View();
            }

            return View(response.Result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HotelUpdateDTO hotelUpdateDto)
        {
            var response = await _hotelService.UpdateAsync<HotelDTO, HotelUpdateDTO>(hotelUpdateDto);
            if (!response.IsSuccess)
            {
                AddModelErrors(response.ErrorMessages);
                return View(hotelUpdateDto);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _hotelService.GetAsync<HotelDTO, int>(id);
            if (!response.IsSuccess)
            {
                AddModelErrors(response.ErrorMessages);
                return View();
            }

            return View(response.Result);
        }

        [HttpPost]
        public async Task<IActionResult> PostDelete(int id)
        {
            var response = await _hotelService.DeleteAsync<HotelDTO, int>(id);
            if (!response.IsSuccess)
            {
                AddModelErrors(response.ErrorMessages);
                return View();
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
