using Domain;
using System;
using System.Collections.Generic;
using Infrastructure;

namespace Application
{
    public interface IReaderService
    {
        Task<CreateReaderRequest> AddReader(Reader reader);
        Task<bool> UpdateReader(CreateReaderRequest reader);
        Task<bool> DeleteReader(int id);
        Task<CreateReaderRequest> GetReaderById(int id);
        Task<IEnumerable<CreateReaderRequest>> GetAllReaders();
    }
}
