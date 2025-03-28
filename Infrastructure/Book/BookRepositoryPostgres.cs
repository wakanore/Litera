using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Domain;

namespace Infrastructure
{
    public class BookRepository : IBookRepository
    {
        private readonly IDbConnection _db;

        public BookRepository(IDbConnection db)
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

            return true; // Явно возвращаем true при успешном обновлении
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
            const string sql = @"
                SELECT b.*, 
                       a.Id, a.Name, a.Description, a.Phone,
                       r.Id, r.Name, r.Email, r.BookId
                FROM Books b
                LEFT JOIN Authors a ON b.AuthorId = a.Id
                LEFT JOIN Readers r ON b.Id = r.BookId
                WHERE b.Id = @Id;";

            var bookDict = new Dictionary<int, Book>();

            var result = await _db.QueryAsync<Book, Author, Reader, Book>(
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
                new { Id = id },
                splitOn: "Id,Id");

            return bookDict.Values.FirstOrDefault()
                ?? throw new InvalidOperationException("Book not found.");
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            const string sql = @"
                SELECT b.*, 
                       a.Id, a.Name, a.Description, a.Phone,
                       r.Id, r.Name, r.Email, r.BookId
                FROM Books b
                LEFT JOIN Authors a ON b.AuthorId = a.Id
                LEFT JOIN Readers r ON b.Id = r.BookId
                ORDER BY b.Name;";

            var bookDict = new Dictionary<int, Book>();

            await _db.QueryAsync<Book, Author, Reader, Book>(
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

        public async Task InitializeData()
        {
            var count = await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Books;");
            if (count > 0) return;

            var initialBooks = new List<Book>
            {
                new Book
                {
                    Name = "War and Peace",
                    Style = "Novel",
                    AuthorId = 1,
                    CreatedDate = DateTime.UtcNow,
                    LinkToCover = "/covers/war_and_peace.jpg"
                },
                new Book
                {
                    Name = "Crime and Punishment",
                    Style = "Psychological",
                    AuthorId = 2,
                    CreatedDate = DateTime.UtcNow,
                    LinkToCover = "/covers/crime_and_punishment.jpg"
                }
            };

            foreach (var book in initialBooks)
            {
                await Add(book);
            }
        }
    }
}