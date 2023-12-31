﻿using HotelManagment_API.Data;
using HotelManagment_API.Models;
using HotelManagment_API.Repository.Interfaces;

namespace HotelManagment_API.Repository
{
    public class TokenRepository : Repository<Token>, ITokenRepository
    {
        public TokenRepository(ApplicationDbContext db) : base(db) { }

        public async Task UpdateAsync(Token token)
        {
            _db.Update(token);
            await SaveAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<Token> tokens)
        {
            _db.UpdateRange(tokens);
            await SaveAsync();
        }
    }
}
