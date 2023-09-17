using HotelManagment_MVC.Models.DTO.Hotel;
using HotelManagment_MVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            var response = await _hotelService.GetAllAsync();
            if (!response.IsSuccess)
            {
                return View(new List<HotelDTO>());
            }

            var getResponse = JsonConvert.DeserializeObject<APIResponse<List<HotelDTO>>>(response.Result) ?? new APIResponse<List<HotelDTO>>();
            if (!getResponse.IsSuccess)
            {
                return View(new List<HotelDTO>());
            }

            return View(getResponse.Result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(HotelCreateDTO hotelCreateDto)
        {
            var response = await _hotelService.CreateAsync(hotelCreateDto);
            var putResponse = JsonConvert.DeserializeObject<APIResponse<List<HotelDTO>>>(response.Result);
            if (!putResponse.IsSuccess)
            {
                AddModelErrors(putResponse.ErrorMessages);
                return View(hotelCreateDto);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var response = await _hotelService.GetAsync(id);
            var getResponse = JsonConvert.DeserializeObject<APIResponse<HotelUpdateDTO>>(response.Result) ?? new APIResponse<HotelUpdateDTO>();
            if (!getResponse.IsSuccess)
            {
                AddModelErrors(getResponse.ErrorMessages);
                return View();
            }

            return View(getResponse.Result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HotelUpdateDTO hotelUpdateDto)
        {
            var response = await _hotelService.UpdateAsync(hotelUpdateDto);
            var getResponse = JsonConvert.DeserializeObject<APIResponse<HotelUpdateDTO>>(response.Result) ?? new APIResponse<HotelUpdateDTO>();
            if (!getResponse.IsSuccess)
            {
                AddModelErrors(getResponse.ErrorMessages);
                return View(hotelUpdateDto);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _hotelService.GetAsync(id);
            var getResponse = JsonConvert.DeserializeObject<APIResponse<HotelDTO>>(response.Result) ?? new APIResponse<HotelDTO>();
            if (!getResponse.IsSuccess)
            {
                AddModelErrors(getResponse.ErrorMessages);
                return View();
            }

            return View(getResponse.Result);
        }

        [HttpPost]
        public async Task<IActionResult> PostDelete(int id)
        {
            var deleteResponse = await _hotelService.DeleteAsync(id);
            if (!deleteResponse.IsSuccess)
            {
                AddModelErrors(deleteResponse.ErrorMessages);
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
