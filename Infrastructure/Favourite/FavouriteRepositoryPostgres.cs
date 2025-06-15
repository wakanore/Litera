using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Domain;

namespace Infrastructure
{
    public class FavouritePostgresRepository : IFavouriteRepository
    {
        private readonly IDbConnection _db;

        public FavouritePostgresRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<Favourite> Add(Favourite favourite)
        {
            const string sql = @"
        INSERT INTO Favourites (UserId, BookId)
        VALUES (@UserId, @BookId)
        RETURNING *;"; 

            return await _db.QuerySingleAsync<Favourite>(sql, favourite);
        }

        public async Task<bool> FavouriteExists(int authorId, int readerId)
        {
            const string sql = "SELECT 1 FROM Favourites WHERE UserId = @UserId AND BookId = @BookId";
            return await _db.ExecuteScalarAsync<bool>(sql, new { AuthorId = authorId, ReaderId = readerId });
        }

        public async Task<bool> Delete(int authorId, int readerId)
        {
            const string sql = "DELETE FROM Favourites WHERE UserId = @UserId AND BookId = @BookId";
            var rowsAffected = await _db.ExecuteAsync(sql, new { AuthorId = authorId, ReaderId = readerId });
            return rowsAffected > 0;
        }
    }
}