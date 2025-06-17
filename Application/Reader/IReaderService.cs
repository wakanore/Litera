using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

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
