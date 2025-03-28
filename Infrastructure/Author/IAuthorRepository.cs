using System;
using System.Collections.Generic;
using Domain;

namespace Infrastructure
{
    public interface IAuthorRepository
    {

        Task<Author> Add(Author author); // Делаем метод асинхронным
        Task Update(Author author);
        Task Delete(int id);
        Task<Author> GetById(int id);
        Task<IEnumerable<Author>> GetAll();
    }
}