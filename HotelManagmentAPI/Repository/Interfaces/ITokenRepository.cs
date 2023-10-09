using HotelManagment_API.Models;

namespace HotelManagment_API.Repository.Interfaces
{
    public interface ITokenRepository : IRepository<Token>
    {
        Task UpdateAsync(Token tokens);

        Task UpdateRangeAsync(IEnumerable<Token> tokens);
    }
}
