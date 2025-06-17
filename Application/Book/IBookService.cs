using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Application
{
    public interface IBookService
    {
        void AddBook(BookDto bookDto);
        void UpdateBook(BookDto bookDto);
        void DeleteBook(int id);
        BookDto GetBookById(int id);
        IEnumerable<BookDto> GetAllBooks();
    }
}
