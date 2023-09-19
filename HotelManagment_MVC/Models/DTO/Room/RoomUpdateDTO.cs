﻿namespace HotelManagment_MVC.Models.DTO.Room
{
    public class RoomUpdateDTO
    {
        public int Id { get; set; }

        public string RoomNumber { get; set; }

        public decimal PricePerNight { get; set; }

        public bool IsAvailable { get; set; }

        public int HotelId { get; set; }
    }
}