using System.ComponentModel.DataAnnotations;

namespace HotelManagment_MVC.Models.DTO.Room
{
    public class RoomDTO
    {
        public int Id { get; set; }

        [Display(Name = "Room Number")]
        public string RoomNumber { get; set; }

        [Display(Name = "Price/night")]
        public decimal PricePerNight { get; set; }

        [Display(Name = "Availability")]
        public bool IsAvailable { get; set; }

        public int HotelId { get; set; }

        public Models.Hotel Hotel { get; set; }
    }
}
