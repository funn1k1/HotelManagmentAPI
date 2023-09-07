using HotelManagmentAPI.Models.DTO;

namespace HotelManagmentAPI.Data
{
    public static class HotelStore
    {
        public static List<HotelDTO> Hotels { get; } = new List<HotelDTO>
        {
            new HotelDTO { Id = 1, Name = "Test1" },
            new HotelDTO { Id = 2, Name = "Test2" },
        };
    }
}
