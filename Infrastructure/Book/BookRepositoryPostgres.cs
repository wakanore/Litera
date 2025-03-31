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

        public async Task<Book> Add(BookDto book)
        {
            const string sql = @"
                INSERT INTO Books (Name, Style, AuthorId, CreatedDate, LinkToCover)
                VALUES (@Name, @Style, @AuthorId, @CreatedDate, @LinkToCover)
                RETURNING Id, Name, Style, AuthorId, CreatedDate, LinkToCover;";

            return await _db.QuerySingleAsync<Book>(sql, book);
        }

        public async Task<bool> Update(BookDto book)
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

        public async Task<IEnumerable<BookDto>> GetAll()
        {
            const string sql = @"
                SELECT b.*, 
                       a.Id, a.Name, a.Description, a.Phone,
                       r.Id, r.Name, r.Email, r.BookId
                FROM Books b
                LEFT JOIN Authors a ON b.AuthorId = a.Id
                LEFT JOIN Readers r ON b.Id = r.BookId
                ORDER BY b.Name;";

            var bookDict = new Dictionary<int, BookDto>();

            await _db.QueryAsync<BookDto, Author, Reader, BookDto>(
                sql,
                (book, author, reader) =>
                {
                    if (!bookDict.TryGetValue(book.Id, out var bookEntry))
                    {
                        bookEntry = book;
                        bookEntry.Readers = new List<Reader>();
                        bookDict.Add(bookEntry.Id, bookEntry);
                    }

                    bookEntry.Author = author;
                    if (reader != null)
                        bookEntry.Readers.Add(reader);

                    return bookEntry;
                },
                splitOn: "Id,Id");

            return bookDict.Values;
        }
    }
}