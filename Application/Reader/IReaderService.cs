using Domain;
using System;
using System.Collections.Generic;

namespace Application
{
    public interface IReaderService
    {
        Task<Reader> AddReader(Reader reader);
        Task<bool> UpdateReaderAsync(Reader reader);
        Task<bool> DeleteReaderAsync(int id);
        Task<Reader> GetReaderById(int id);
        Task<IEnumerable<Reader>> GetAllReaders();
    }
}
