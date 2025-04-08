using Domain;
using Infrastructure;
using System;
using System.Collections.Generic;

namespace Application
{
    public interface IBookService
    {
        Task<BookDto> AddBook(BookDto bookDto);
        Task<bool> UpdateBook(BookDto book);
        Task<bool> DeleteBook(int id);
        Task<Book> GetBookById(int id);
        Task<IEnumerable<BookDto>> GetAllBooks();
    }
}
