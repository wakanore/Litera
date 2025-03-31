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

        public  Task<Book> AddBook(BookDto book)
        {
            var addedBook = _bookRepository.Add(book);
            return addedBook;
        }

        public  Task<bool> UpdateBookAsync(BookDto book)
        {
            return _bookRepository.Update(book);
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

        public Task<IEnumerable<BookDto>> GetAllBooks()
        {
            return  _bookRepository.GetAll();
        }
    }
}
