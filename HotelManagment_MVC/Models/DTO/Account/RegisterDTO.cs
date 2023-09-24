using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelManagment_MVC.Models.DTO.Account
{
    public class RegisterDTO
    {
        public string FullName { get; set; }

        public string Role { get; set; }

        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
