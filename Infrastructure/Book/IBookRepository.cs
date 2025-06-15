using System;
using System.Collections.Generic;
using Domain;

namespace Infrastructure
{
    public interface IBookRepository
    {
        Task<Book> Add(Book book);
        Task<bool> Update(Book book);
        Task Delete(int id);
        Task<Book> GetById(int id);
        Task<IEnumerable<Book>> GetAll();
    }
}