﻿using System.ComponentModel.DataAnnotations;

namespace HotelManagment_MVC.Models.DTO.Account
{
    public class UserLoginDTO
    {
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
