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

        public async Task<bool> UpdateBookAsync(BookDto bookDto)
        {
            var domainBook = new Domain.Book
            {
                Id = bookDto.Id,
                Name = bookDto.Name,
                AuthorId = bookDto.Author.Id
            };

            return await _bookRepository.Update(domainBook);
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            try
            {
                await _bookRepository.Delete(id);
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public Task<Book> GetBookById(int id)
        {
            var book = _bookRepository.GetById(id);
            return book;
        }

        public async Task<IEnumerable<BookDto>> GetAllBooks()
        {
            var books = await _bookRepository.GetAll();
            return books.Select(book => new BookDto
            {
                Id = book.Id,
                Name = book.Name
            });
        }
    }
}
