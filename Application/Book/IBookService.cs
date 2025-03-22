using Domain;
using System;
using System.Collections.Generic;

namespace Application
{
    public interface IBookService
    {
        BookDto AddBook(BookDto bookDto);
        bool UpdateBook(BookDto bookDto);
        bool DeleteBook(int id);
        BookDto GetBookById(int id);
        IEnumerable<BookDto> GetAllBooks();
    }
}
