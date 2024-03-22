using AutoMapper;
using HotelManagment_API.Models;
using HotelManagment_API.Models.DTO.Account;
using HotelManagment_API.Models.DTO.Hotel;
using HotelManagment_API.Models.DTO.Room;

namespace HotelManagment_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            // Hotel models
            CreateMap<Hotel, HotelDTO>();
            CreateMap<HotelCreateDTO, Hotel>();
            CreateMap<Hotel, HotelUpdateDTO>().ReverseMap();

            // Room models
            CreateMap<Room, RoomDTO>();
            CreateMap<RoomCreateDTO, Room>();
            CreateMap<Room, RoomUpdateDTO>().ReverseMap();

            // User models
            CreateMap<UserRegisterDTO, ApplicationUser>();
        }
    }
}