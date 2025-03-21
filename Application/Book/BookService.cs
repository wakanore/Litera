using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Infrastructure;

namespace Application
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public void AddBook(BookDto bookDto)
        {
            var Book = new Book
            {
                Name = bookDto.Name
            };
            _bookRepository.Add(Book);
        }
        public void UpdateBook(BookDto bookDto)
        {
            if (bookDto == null)
            {
                throw new ArgumentNullException(nameof(bookDto));
            }

            var existingBook = _bookRepository.GetById(bookDto.Id);
            if (existingBook == null)
            {
                throw new InvalidOperationException("Book not found.");
            }

            existingBook.Name = bookDto.Name;

            _bookRepository.Update(existingBook);
        }

        public void DeleteBook(int id)
        {
            var bookToDelete = _bookRepository.GetById(id);
            if (bookToDelete == null)
            {
                throw new InvalidOperationException("Book not found.");
            }

            _bookRepository.Delete(id);
        }

        public BookDto GetBookById(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                throw new InvalidOperationException("Book not found.");
            }

            return new BookDto
            {
                Id = book.Id,
                Name = book.Name
            };
        }

        public IEnumerable<BookDto> GetAllBooks()
        {
            var books = _bookRepository.GetAll();
            return books.Select(book => new BookDto
            {
                Id = book.Id,
                Name = book.Name
            });
        }
    }
}
