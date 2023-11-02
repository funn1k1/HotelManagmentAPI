using System.ComponentModel.DataAnnotations;

namespace HotelManagment_MVC.Models.DTO.Room
{
    public class RoomUpdateDTO
    {
        public int Id { get; set; }

        [Display(Name = "Room Number")]
        public string RoomNumber { get; set; }

        [Display(Name = "Price/night")]
        public decimal PricePerNight { get; set; }

        [Display(Name = "Available")]
        public bool IsAvailable { get; set; }

        [Display(Name = "Hotel")]
        public int HotelId { get; set; }
    }
}
