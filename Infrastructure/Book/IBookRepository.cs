using System;
using System.Collections.Generic;
using Domain;

namespace Infrastructure
{
    public interface IBookRepository
    {
        Book Add(Book book);
        void Update(Book book);
        void Delete(int id);
        Book GetById(int id);
        IEnumerable<Book> GetAll();
    }
}