using System;
using System.Collections.Generic;
using Domain;

namespace Infrastructure
{
    public interface IBookRepository
    {
        Task<Book> Add(BookDto book);
        Task<bool> Update(BookDto book);
        Task Delete(int id);
        Task<Book> GetById(int id);
        Task<IEnumerable<BookDto>> GetAll();
    }
}