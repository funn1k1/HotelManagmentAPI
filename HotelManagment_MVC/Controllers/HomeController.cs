using System.Diagnostics;
using HotelManagment_MVC.Models;
using HotelManagment_MVC.Models.DTO.Hotel;
using HotelManagment_MVC.Services.Interfaces;
using HotelManagment_MVC.ViewModels.Hotel;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagment_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHotelService _hotelService;

        public HomeController(ILogger<HomeController> logger, IHotelService hotelService)
        {
            _logger = logger;
            _hotelService = hotelService;
        }

        public async Task<IActionResult> Index()
        {
            var getHotelsResp = await _hotelService.GetAllAsync<List<HotelDTO>>();
            var hotelVM = new HotelViewModel
            {
                Hotels = getHotelsResp.Result
            };
            return View(hotelVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
