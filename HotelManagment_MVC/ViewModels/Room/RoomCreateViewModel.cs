using HotelManagment_MVC.Models.DTO.Room;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelManagment_MVC.ViewModels.Room
{
    public class RoomCreateViewModel
    {
        public RoomCreateDTO Room { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Hotels { get; set; }
    }
}
