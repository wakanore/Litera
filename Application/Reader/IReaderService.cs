using Domain;
using System;
using System.Collections.Generic;
using Infrastructure;

namespace Application
{
    public interface IReaderService
    {
        Task<ReaderResponse> CreateReader(CreateReaderRequest request);
        Task<ReaderResponse> UpdateReader(UpdateReaderRequest request);
        Task<bool> DeleteReader(int id);
        Task<ReaderResponse> GetReaderById(int id);
        Task<IEnumerable<ReaderResponse>> GetAllReaders();
    }
}
