using System.ComponentModel.DataAnnotations;

namespace HotelManagmentAPI.Models.DTO
{
    public class HotelUpdateDTO
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public decimal Rating { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}
