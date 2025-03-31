using Domain;
using Infrastructure;
using System;
using System.Collections.Generic;

namespace Application
{
    public interface IBookService
    {
        Task<Book> AddBook(BookDto bookDto);
        Task<bool> UpdateBookAsync(BookDto book);
        Task<bool> DeleteBookAsync(int id);
        Task<Book> GetBookById(int id);
        Task<IEnumerable<BookDto>> GetAllBooks();
    }
}
