using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Domain;

namespace Infrastructure
{
    public class AuthorPostgresRepository : IAuthorRepository
    {
        private readonly IDbConnection _db;

        public AuthorPostgresRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<Author> Add(Author author)
        {
            const string sql = @"
                INSERT INTO Users (Id, Name, Description, Phone)
                VALUES (@Id, @Name, @Description, @Phone)
                RETURNING Id, Name, Description, Phone;";

            var insertedAuthor = await _db.QuerySingleAsync<Author>(sql, author);
            return insertedAuthor;
        }

        public async Task Update(Author author)
        {
            const string sql = @"
                UPDATE Users 
                SET Name = @Name, 
                    Description = @Description,
                    Phone = @Phone
                WHERE Id = @Id;";

            var affectedRows = await _db.ExecuteAsync(sql, author);
            if (affectedRows == 0)
            {
                throw new InvalidOperationException("Author not found.");
            }
        }

        public async Task Delete(int id)
        {
            const string sql = "DELETE FROM Users WHERE Id = @Id;";

            var affectedRows = await _db.ExecuteAsync(sql, new { Id = id });
            if (affectedRows == 0)
            {
                throw new InvalidOperationException("Author not found.");
            }
        }

        public async Task<Author> GetById(int id)
        {
            const string sql = "SELECT id, name, phone, description FROM Users WHERE Id = @Id;";

            var author = await _db.QuerySingleOrDefaultAsync<Author>(sql, new { Id = id });
            if (author == null)
            {
                throw new InvalidOperationException("Author not found.");
            }

            return author;
        }

        public async Task<IEnumerable<Author>> GetAll()
        {
            const string sql = "SELECT id, name, phone, description FROM Users ORDER BY Name;";
            return await _db.QueryAsync<Author>(sql);
        }
    }
}