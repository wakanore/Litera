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
        private readonly ILogger<ReaderService> _logger;

        public ReaderService(IReaderRepository readerRepository)
        {
            _readerRepository = readerRepository ?? throw new ArgumentNullException(nameof(readerRepository));
        }

        public Task<ReaderDto> AddReader(ReaderDto reader)
        {
            try
            {
                var addedReader =  _readerRepository.Add(reader);
                return addedReader;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public  Task<bool> UpdateReaderAsync(ReaderDto reader)
        {
            
            return _readerRepository.Update(reader);  
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
            try
            {
                var reader = await _readerRepository.GetById(id);
                return reader;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public  Task<IEnumerable<ReaderDto>> GetAllReaders()
        {
            try
            {
                return  _readerRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}