using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Domain;

namespace Infrastructure
{
    public class BookPostgresRepository : IBookRepository
    {
        private readonly IDbConnection _db;

        public BookPostgresRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<Book> Add(Book book)
        {
            const string sql = @"
                INSERT INTO Books (Name, Style, AuthorId, CreatedDate, LinkToCover)
                VALUES (@Name, @Style, @AuthorId, @CreatedDate, @LinkToCover)
                RETURNING Id, Name, Style, AuthorId, CreatedDate, LinkToCover;";

            return await _db.QuerySingleAsync<Book>(sql, book);
        }

        public async Task<bool> Update(Book book)
        {
            const string sql = @"
        UPDATE Books 
        SET Name = @Name, 
            Style = @Style,
            AuthorId = @AuthorId,
            CreatedDate = @CreatedDate,
            LinkToCover = @LinkToCover
        WHERE Id = @Id;";

            var affectedRows = await _db.ExecuteAsync(sql, book);

            if (affectedRows == 0)
            {
                throw new InvalidOperationException("Book not found.");
            }

            return true; 
        }

        public async Task Delete(int id)
        {
            const string sql = "DELETE FROM Books WHERE Id = @Id;";
            var affectedRows = await _db.ExecuteAsync(sql, new { Id = id });
            if (affectedRows == 0)
                throw new InvalidOperationException("Book not found.");
        }

        public async Task<Book> GetById(int id)
        {
            const string sql = "SELECT * FROM Books WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<Book>(sql, new { Id = id })
                ?? throw new KeyNotFoundException("Book not found");
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            var books = await _db.QueryAsync<Book>("SELECT * FROM Books ORDER BY Name");

            var authors = await _db.QueryAsync<Author>("SELECT * FROM Authors WHERE Id IN (SELECT AuthorId FROM Books)");

            var readers = await _db.QueryAsync<Reader>("SELECT * FROM Readers WHERE BookId IN (SELECT Id FROM Books)");

            return books.Select(book =>
            {
                book.AuthorId = authors.FirstOrDefault(a => a.Id == book.AuthorId)?.Id ?? 0;
                return book;
            });
        }
    }
}