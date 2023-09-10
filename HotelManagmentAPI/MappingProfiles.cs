using AutoMapper;
using HotelManagmentAPI.Models;
using HotelManagmentAPI.Models.DTO.Hotel;
using HotelManagmentAPI.Models.DTO.Room;

namespace HotelManagmentAPI
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Hotel models
            CreateMap<Hotel, HotelDTO>();
            CreateMap<HotelCreateDTO, Hotel>();
            CreateMap<Hotel, HotelUpdateDTO>().ReverseMap();

            // Room models
            CreateMap<Room, RoomDTO>();
            CreateMap<RoomCreateDTO, Room>();
            CreateMap<Room, RoomUpdateDTO>().ReverseMap();
        }
    }
}