using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class BookRepository : IBookRepository
    {
        private readonly List<Domain.Book> _books = new List<Domain.Book>();
        public void Add(Domain.Book book)
        {
            if (_books.Any(b => b.Id == book.Id))
            {
                throw new InvalidOperationException("Book with the same ID already exists.");
            }

            _books.Add(book);
        }

        public void Update(Domain.Book book)
        {
            var existingBook = _books.FirstOrDefault(b => b.Id == book.Id);
            if (existingBook == null)
            {
                throw new InvalidOperationException("Book not found.");
            }


            existingBook.Name = book.Name;
            existingBook.AuthorId = book.AuthorId;
        }

        public void Delete(int id)
        {
            var bookToDelete = _books.FirstOrDefault(b => b.Id == id);
            if (bookToDelete == null)
            {
                throw new InvalidOperationException("Book not found.");
            }

            _books.Remove(bookToDelete);
        }
        public Domain.Book GetById(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                throw new InvalidOperationException("Book not found.");
            }

            return book;
        }
        public IEnumerable<Domain.Book> GetAll()
        {
            return _books;
        }

    }
}