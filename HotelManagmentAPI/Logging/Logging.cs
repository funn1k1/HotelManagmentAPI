using HotelManagmentAPI.Logging.Enums;
using HotelManagmentAPI.Logging.Interfaces;

namespace HotelManagmentAPI.Logging
{
    public class Logging : ILogging
    {
        public void Log(LogLevels level, string message)
        {
            switch (level)
            {
                case LogLevels.Warning:
                    OutputMessage(ConsoleColor.Green, LogLevels.Warning, message);
                    break;
                case LogLevels.Error:
                    OutputMessage(ConsoleColor.Red, LogLevels.Error, message);
                    break;
                case LogLevels.Information:
                    OutputMessage(ConsoleColor.Green, LogLevels.Information, message);
                    break;
            }
        }

        public void OutputMessage(ConsoleColor color, LogLevels logLevel, string message)
        {
            Console.ForegroundColor = color;
            Console.Write($"[{logLevel}] - {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
