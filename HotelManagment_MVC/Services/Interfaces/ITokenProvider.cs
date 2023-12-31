﻿using HotelManagment_MVC.Models.DTO.Account;

namespace HotelManagment_MVC.Services.Interfaces
{
    public interface ITokenProvider
    {
        TokenDTO GetToken();

        void SetToken(TokenDTO tokenDto);

        void DeleteToken();
    }
}
