using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HotelManagmentAPI.Models
{
    public class Room
    {
        public int Id { get; set; }

        public string RoomNumber { get; set; }

        public decimal PricePerNight { get; set; }

        public bool IsAvailable { get; set; }

        [ForeignKey(nameof(Hotel))]
        public int HotelId { get; set; }

        public Hotel Hotel { get; set; }
    }
}
