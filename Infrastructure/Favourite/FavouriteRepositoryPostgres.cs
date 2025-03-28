using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Domain;

namespace Infrastructure
{
    public class FavouriteRepository : IFavouriteRepository
    {
        private readonly IDbConnection _db;

        public FavouriteRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<Favourite> Add(Favourite favourite)
        {
            const string sql = @"
                INSERT INTO Favourites (AuthorId, ReaderId)
                VALUES (@AuthorId, @ReaderId)
                RETURNING AuthorId, ReaderId;";

            return await _db.QuerySingleAsync<Favourite>(sql, favourite);
        }

        public async Task Delete(Favourite favourite)
        {
            const string sql = @"
                DELETE FROM Favourites 
                WHERE AuthorId = @AuthorId AND ReaderId = @ReaderId;";

            var affectedRows = await _db.ExecuteAsync(sql, favourite);
            if (affectedRows == 0)
            {
                throw new InvalidOperationException("Favourite not found.");
            }
        }
    }
}