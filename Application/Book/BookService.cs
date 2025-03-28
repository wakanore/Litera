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

        public async Task<Book> AddBook(Book book)
        {
            var addedBook = await _bookRepository.Add(book);
            return addedBook;
        }

        public async Task<bool> UpdateBookAsync(Book book)
        {
            try
            {
                await _bookRepository.Update(book);
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
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

        public async Task<Book> GetBookById(int id)
        {
            var book = await _bookRepository.GetById(id);
            Console.WriteLine($"Retrieved author: {book.Name}");
            return book;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _bookRepository.GetAll();
        }

        public async Task InitializeBooksData()
        {
            await _bookRepository.InitializeData();
        }
    }
}
