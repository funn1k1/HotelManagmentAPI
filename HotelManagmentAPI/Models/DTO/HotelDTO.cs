using System.ComponentModel.DataAnnotations;

namespace HotelManagmentAPI.Models.DTO
{
    public class HotelDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public string Address { get; set; }
    }
}
