using Domain;
using System;
using System.Collections.Generic;

namespace Infrastructure
{
    public interface IReaderRepository
    {
        Task<Reader> Add(Reader reader);
        Task<bool> Update(Reader reader);
        Task Delete(int id);
        Task<Reader> GetById(int id);
        Task<IEnumerable<Reader>> GetAll();
    }
}