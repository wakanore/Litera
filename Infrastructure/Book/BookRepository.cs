using System;
using System.Collections.Generic;
using System.Linq;
using Domain;

namespace Infrastructure
{
    public class BookRepository : IBookRepository
    {
        private readonly List<Book> _books = new List<Book>();

        public BookRepository()
        {
            _books.Add(new Book { Id = 1, Name = "War and Peace", AuthorId = 1 });
            _books.Add(new Book { Id = 2, Name = "Anna Karenina", AuthorId = 1 });
            _books.Add(new Book { Id = 3, Name = "Crime and Punishment", AuthorId = 2 });
            _books.Add(new Book { Id = 4, Name = "The Brothers Karamazov", AuthorId = 2 });
            _books.Add(new Book { Id = 5, Name = "The Cherry Orchard", AuthorId = 3 });
        }

        public Book Add(Book book)
        {
            book.Id = _books.Any() ? _books.Max(b => b.Id) + 1 : 1;
            _books.Add(book);
            return book;
        }

        public void Update(Book book)
        {
            var existingBook = _books.FirstOrDefault(b => b.Id == book.Id);
            if (existingBook == null)
            {
                throw new InvalidOperationException("Book not found.");
            }

            existingBook.Name = book.Name;
            existingBook.AuthorId = book.AuthorId;
            existingBook.Style = book.Style;
            existingBook.LinkToCover = book.LinkToCover;
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

        public Book GetById(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                throw new InvalidOperationException("Book not found.");
            }
            return book;
        }

        public IEnumerable<Book> GetAll()
        {
            return _books;
        }
    }
}