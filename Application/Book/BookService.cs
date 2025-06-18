using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application;
using Domain;
using Infrastructure;

namespace Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<BookDto> AddBook(BookDto bookDto)
        {
            var domainBook = new Book
            {
                Name = bookDto.Name,
                AuthorId = bookDto.Author.Id
            };

            var addedBook = await _bookRepository.Add(domainBook);

            return new BookDto
            {
                Id = addedBook.Id,
                Name = addedBook.Name
            };
        }

        public Task<bool> UpdateBook(BookDto bookDto)
        {
            var domainBook = new Book
            {
                Id = bookDto.Id,
                Name = bookDto.Name,
                AuthorId = bookDto.Author.Id
            };

            return _bookRepository.Update(domainBook);
        }

        public Task<bool> DeleteBook(int id)
        {
            return _bookRepository.Delete(id)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted) 
                        return false;
                    return true;
                });
        }

        public async Task<BookDto> GetBookById(int id)
        {
            var book = await _bookRepository.GetById(id);

            return new BookDto
            {
                Id = book.Id,
                Name = book.Name
            };
        }

        public async Task<IEnumerable<BookDto>> GetAllBooks()
        {
            try
            {
                var books = await _bookRepository.GetAll();
                return books.Select(book => new BookDto
                {
                    Id = book.Id,
                    Name = book.Name
                });
            }
            catch
            {
                return Enumerable.Empty<BookDto>();
            }
        }
    }
}
