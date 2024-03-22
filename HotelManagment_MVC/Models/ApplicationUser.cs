using Microsoft.AspNetCore.Identity;

namespace HotelManagment_MVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
