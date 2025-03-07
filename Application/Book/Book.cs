using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Application
{

    public class BookDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class BookService
    {
        private readonly Infrastructure.IBookRepository _bookRepository;

        public BookService(Infrastructure.IBookRepository BookRepository)
        {
            _bookRepository = BookRepository;
        }


        public void AddBook(BookDto BookDto)
        {
            var Book = new Domain.Book
            {
                Name = BookDto.Name
            };
            _bookRepository.Add(Book);
        }
    }
}
