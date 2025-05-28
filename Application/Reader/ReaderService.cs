using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Infrastructure;
using Microsoft.Extensions.Logging;
using static System.Reflection.Metadata.BlobBuilder;

namespace Application
{
    public class ReaderService : IReaderService
    {
        private readonly IReaderRepository _readerRepository;

        public ReaderService(IReaderRepository readerRepository)
        {
            _readerRepository = readerRepository;
        }

        public async Task<CreateReaderRequest> AddReader(Reader readerDto)
        {
            var readerEntity = new Reader
            {
                Id = readerDto.Id,
                Name = readerDto.Name,
                Phone = readerDto.Phone,
            };

            var addedReader = await _readerRepository.Add(readerEntity);

            return new CreateReaderRequest(
                addedReader.Id,
                addedReader.Name,
                addedReader.Phone
            );
        }

        public Task<bool> UpdateReader(CreateReaderRequest readerDto)
        {
            var reader = new Reader
            {
                Id = readerDto.Id,
                Name = readerDto.Name,
                Phone = readerDto.Phone
            };

            return _readerRepository.Update(reader);
        }

        public async Task<bool> DeleteReader(int id)
        {
            try
            {
                await _readerRepository.Delete(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<CreateReaderRequest> GetReaderById(int id)
        {
            var readerEntity = await _readerRepository.GetById(id);

            var readerDto = new CreateReaderRequest(
                    Id: readerEntity.Id,
                    Name: readerEntity.Name,
                    Phone: readerEntity.Phone
                );

            return readerDto;
        }

        public async Task<IEnumerable<CreateReaderRequest>> GetAllReaders()
        {
            try
            {
                var readers = await _readerRepository.GetAll();
                return readers.Select(reader => new CreateReaderRequest(
                    Id: reader.Id,
                    Name: reader.Name,
                    Phone: reader.Phone 
                ));

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}