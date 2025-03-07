using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Application
{
    public class ReaderDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }

    public class ReaderService
    {
        private readonly Infrastructure.IReaderRepository _readerRepository;

        public ReaderService(Infrastructure.IReaderRepository ReaderRepository)
        {
            _readerRepository = ReaderRepository;
        }


        public void AddReader(ReaderDto ReaderDto)
        {
            var Reader = new Domain.Reader
            {
                Name = ReaderDto.Name,
                Phone = ReaderDto.Phone
            };
            _readerRepository.Add(Reader);
        }
    }

}
