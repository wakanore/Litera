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


        public void AddReader(ReaderDto readerDto)
        {
            if (readerDto == null)
            {
                throw new ArgumentNullException(nameof(readerDto), "ReaderDto cannot be null.");
            }
            var Reader = new Reader
            {
                Name = readerDto.Name,
                Phone = readerDto.Phone
            };
            _readerRepository.Add(Reader);
        }

        public void UpdateReader(ReaderDto readerDto)
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
        }

        public void DeleteReader(int id)
        {
            var readerToDelete = _readerRepository.GetById(id);
            if (readerToDelete == null)
            {
                throw new InvalidOperationException("Reader not found.");
            }

            _readerRepository.Delete(id);
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
