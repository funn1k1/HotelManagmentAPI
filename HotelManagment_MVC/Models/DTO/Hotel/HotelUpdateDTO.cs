﻿using System.ComponentModel.DataAnnotations;

namespace HotelManagment_MVC.Models.DTO.Hotel
{
    public class HotelUpdateDTO
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public decimal Rating { get; set; }

        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
