using Domain;
using System;
using System.Collections.Generic;

namespace Application
{
    public interface IBookService
    {
        Task<Book> AddBook(Book bookDto);
        Task<bool> UpdateBookAsync(Book book);
        Task<bool> DeleteBookAsync(int id);
        Task<Book> GetBookById(int id);
        Task<IEnumerable<Book>> GetAllBooks();
        Task InitializeBooksData();
    }
}
