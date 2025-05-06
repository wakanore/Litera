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

        public async Task<CreateBookRequest> AddBook(CreateBookRequest bookDto)
        {
            var domainBook = new Book
            {
                Name = bookDto.Name,
                AuthorId = bookDto.Author.Id
            };

            var addedBook = await _bookRepository.Add(domainBook);

            return new CreateBookRequest(
                Id: addedBook.Id,
                Name: addedBook.Name,
                Readers: new List<CreateReaderRequest>(),
                Author: new CreateAuthorRequest(
                    Id: 0, // or appropriate default value
                    Name: "", // or appropriate default value
                    Phone: "" // or appropriate default value
                )
            );
        }

        public Task<bool> UpdateBook(CreateBookRequest bookDto)
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

        public async Task<CreateBookRequest> GetBookById(int id)
        {
            var book = await _bookRepository.GetById(id);

            return new CreateBookRequest(
                book.Id,
                book.Name,
                new List<CreateReaderRequest>(),
                new CreateAuthorRequest(0, "", "")  // Provide required author params
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
                    new List<CreateReaderRequest>(),
                    new CreateAuthorRequest(0, "", "")  // Default values
                ));
            }
            catch
            {
                return Enumerable.Empty<CreateBookRequest>();
            }
        }
    }
}
