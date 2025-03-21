using System;
using System.Collections.Generic;
using Domain;

namespace Infrastructure
{
    public interface IAuthorRepository
    {
        void Add(Author user);
        void Update(Author user);
        void Delete(int id);
        Author GetById(int id);
        IEnumerable<Author> GetAll();
    }
}