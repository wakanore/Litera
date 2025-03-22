using Domain;
using System;
using System.Collections.Generic;

namespace Infrastructure
{
    public interface IReaderRepository
    {
        Reader Add(Reader reader);
        void Update(Reader reader);
        void Delete(int id);
        Reader GetById(int id);
        IEnumerable<Reader> GetAll();
    }
}