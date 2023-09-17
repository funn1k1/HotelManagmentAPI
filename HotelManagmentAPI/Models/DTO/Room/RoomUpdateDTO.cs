﻿namespace HotelManagment_API.Models.DTO.Room
{
    public class RoomUpdateDTO
    {
        public string RoomNumber { get; set; }

        public decimal PricePerNight { get; set; }

        public bool IsAvailable { get; set; }

        public int HotelId { get; set; }
    }
}
