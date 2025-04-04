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

        public async Task<bool> UpdateReaderAsync(ReaderDto readerDto)
        {
            var reader = new Domain.Reader
            {
                Id = readerDto.Id,
                Name = readerDto.Name,
                Phone = readerDto.Phone
            };

            return await _readerRepository.Update(reader);
        }

        public async Task<bool> DeleteReaderAsync(int id)
        {
            try
            {
                await _readerRepository.Delete(id);
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ReaderDto> GetReaderById(int id)
        {
            var readerEntity = await _readerRepository.GetById(id);

            if (readerEntity == null)
            {
                return null;
            }

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