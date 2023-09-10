﻿using AutoMapper;
using HotelManagmentAPI.Models;
using HotelManagmentAPI.Models.DTO.Hotel;

namespace HotelManagmentAPI
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Hotel, HotelDTO>();
            CreateMap<HotelCreateDTO, Hotel>();
            CreateMap<Hotel, HotelUpdateDTO>().ReverseMap();
        }
    }
}