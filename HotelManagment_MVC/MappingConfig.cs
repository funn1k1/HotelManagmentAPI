using AutoMapper;
using HotelManagment_MVC.Models.DTO.Hotel;

namespace HotelManagment_MVC
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<HotelDTO, HotelUpdateDTO>();
        }
    }
}