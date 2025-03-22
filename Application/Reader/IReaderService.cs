using System;
using System.Collections.Generic;

namespace Application
{
    public interface IReaderService
    {
        ReaderDto AddReader(ReaderDto readerDto);
        bool UpdateReader(ReaderDto readerDto);
        bool DeleteReader(int id);
        ReaderDto GetReaderById(int id);
        IEnumerable<ReaderDto> GetAll();
    }
}
