using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using Domain;


namespace Application
{
    public class ReaderService : IReaderService
    {
        private readonly IReaderRepository _readerRepository;

        public ReaderService(IReaderRepository readerRepository)
        {
            _readerRepository = readerRepository;
        }


        public ReaderDto AddReader(ReaderDto readerDto)
        {
            var reader = new Reader
            {
                Name = readerDto.Name,
                Phone = readerDto.Phone
            };

            var createdReader = _readerRepository.Add(reader);


            return new ReaderDto
            {
                Id = createdReader.Id,
                Name = createdReader.Name,
                Phone = createdReader.Phone
            };
        }

        public bool UpdateReader(ReaderDto readerDto)
        {
            if (readerDto == null)
            {
                throw new ArgumentNullException(nameof(readerDto));
            }

            var existingReader = _readerRepository.GetById(readerDto.Id);
            if (existingReader == null)
            {
                throw new InvalidOperationException("Reader not found.");
            }

            existingReader.Name = readerDto.Name;
            existingReader.Phone = readerDto.Phone;

            _readerRepository.Update(existingReader);
            return true;
        }

        public bool DeleteReader(int id)
        {
            var readerToDelete = _readerRepository.GetById(id);
            if (readerToDelete == null)
            {
                throw new InvalidOperationException("Reader not found.");
            }

            _readerRepository.Delete(id);
            return true;
        }

        public ReaderDto GetReaderById(int id)
        {
            var reader = _readerRepository.GetById(id);
            if (reader == null)
            {
                throw new InvalidOperationException("Reader not found.");
            }

            return new ReaderDto
            {
                Id = reader.Id,
                Name = reader.Name,
                Phone = reader.Phone
            };
        }

        public IEnumerable<ReaderDto> GetAll()
        {
            var readers = _readerRepository.GetAll();
            return readers.Select(reader => new ReaderDto
            {
                Id = reader.Id,
                Name = reader.Name,
                Phone = reader.Phone
            });
        }
    }
}
