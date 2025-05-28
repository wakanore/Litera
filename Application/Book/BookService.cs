using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<CreateBookRequest> AddBook(Book bookDto)
        {
            var domainBook = new Book
            {
                Id = bookDto.Id,
                Name = bookDto.Name,
                AuthorId = bookDto.AuthorId
            };

            var addedBook = await _bookRepository.Add(domainBook);

            return new CreateBookRequest(
                Id: addedBook.Id,
                Name: addedBook.Name,
                Style: addedBook.Style,
                AuthorId: addedBook.AuthorId
            );
        }

        public Task<bool> UpdateBook(CreateBookRequest bookDto)
        {
            var domainBook = new Book
            {
                Id = bookDto.Id,
                Name = bookDto.Name,
                AuthorId = bookDto.AuthorId
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

        public async Task<CreateBookRequest> GetBookById(int id)
        {
            var book = await _bookRepository.GetById(id);

            return new CreateBookRequest(
                book.Id,
                book.Name,
                book.Style,
                book.AuthorId
            );
        }

        public async Task<IEnumerable<CreateBookRequest>> GetAllBooks()
        {
            try
            {
                var books = await _bookRepository.GetAll();
                return books.Select(book => new CreateBookRequest(
                    book.Id,
                    book.Name,
                    book.Style,
                    book.AuthorId
                ));
            }
            catch
            {
                return Enumerable.Empty<CreateBookRequest>();
            }
        }
    }
}
