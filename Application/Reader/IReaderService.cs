using System;
using System.Collections.Generic;

namespace Application
{
    public interface IReaderService
    {
        void AddReader(ReaderDto readerDto);
        void UpdateReader(ReaderDto readerDto);
        void DeleteReader(int id);
        ReaderDto GetReaderById(int id);
        IEnumerable<ReaderDto> GetAll();
    }
}
