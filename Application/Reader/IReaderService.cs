using Domain;
using System;
using System.Collections.Generic;
using Infrastructure;

namespace Application
{
    public interface IReaderService
    {
        Task<ReaderDto> AddReader(ReaderDto reader);
        Task<bool> UpdateReaderAsync(ReaderDto reader);
        Task<bool> DeleteReaderAsync(int id);
        Task<ReaderDto> GetReaderById(int id);
        Task<IEnumerable<ReaderDto>> GetAllReaders();
    }
}
