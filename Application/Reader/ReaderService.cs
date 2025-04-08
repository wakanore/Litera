using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Infrastructure;
using Microsoft.Extensions.Logging;

namespace Application
{
    public class ReaderService : IReaderService
    {
        private readonly IReaderRepository _readerRepository;

        public ReaderService(IReaderRepository readerRepository)
        {
            _readerRepository = readerRepository;
        }

        public async Task<ReaderDto> AddReader(ReaderDto readerDto)
        {
            var readerEntity = new Reader
            {
                Name = readerDto.Name,
                Phone = readerDto.Phone,
            };

            var addedReader = await _readerRepository.Add(readerEntity);

            return new ReaderDto
            {
                Id = addedReader.Id,
                Name = addedReader.Name,
                Phone = addedReader.Phone
            };
        }

        public Task<bool> UpdateReader(ReaderDto readerDto)
        {
            var reader = new Domain.Reader
            {
                Id = readerDto.Id,
                Name = readerDto.Name,
                Phone = readerDto.Phone
            };

            return _readerRepository.Update(reader);
        }

        public Task<bool> DeleteReader(int id)
        {
            return _readerRepository.Delete(id)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                        return false;
                    return true;
                });
        }

        public async Task<ReaderDto> GetReaderById(int id)
        {
            var readerEntity = await _readerRepository.GetById(id);

            var readerDto = new ReaderDto
            {
                Id = readerEntity.Id,
                Name = readerEntity.Name,
                Phone = readerEntity.Phone
            };

            return readerDto;
        }

        public async Task<IEnumerable<ReaderDto>> GetAllReaders()
        {
            try
            {
                var readers = await _readerRepository.GetAll();
                return readers.Select(reader => new ReaderDto
                {
                    Id = reader.Id,
                    Name = reader.Name,
                    Phone = reader.Phone
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}