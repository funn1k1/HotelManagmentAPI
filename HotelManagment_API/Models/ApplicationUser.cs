using Microsoft.AspNetCore.Identity;

namespace HotelManagment_API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
