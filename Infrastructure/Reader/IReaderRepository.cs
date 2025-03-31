using Domain;
using System;
using System.Collections.Generic;

namespace Infrastructure
{
    public interface IReaderRepository
    {
        Task<ReaderDto> Add(ReaderDto reader);
        Task<bool> Update(ReaderDto reader);
        Task Delete(int id);
        Task<ReaderDto> GetById(int id);
        Task<IEnumerable<ReaderDto>> GetAll();
    }
}