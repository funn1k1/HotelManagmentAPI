using HotelManagmentAPI.Logging.Enums;

namespace HotelManagmentAPI.Logging.Interfaces
{
    public interface ILogging
    {
        void Log(LogLevels level, string message);
    }
}
