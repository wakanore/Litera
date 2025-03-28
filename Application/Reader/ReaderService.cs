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

        public ReaderService(IReaderRepository readerRepository, ILogger<ReaderService> logger)
        {
            _readerRepository = readerRepository ?? throw new ArgumentNullException(nameof(readerRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Reader> AddReader(Reader reader)
        {
            try
            {
                var addedReader = await _readerRepository.Add(reader);
                _logger.LogInformation($"Added reader: {addedReader.Name}");
                return addedReader;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding reader");
                throw;
            }
        }

        public async Task<bool> UpdateReaderAsync(Reader reader)
        {
            try
            {
                await _readerRepository.Update(reader);
                return true;
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning($"Reader not found: {reader.Id}");
                return false;
            }
            catch (InvalidOperationException)
            {
                _logger.LogWarning($"Reader not found: {reader.Id}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating reader: {reader.Id}");
                throw;
            }
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
                _logger.LogWarning($"Reader not found: {id}");
                return false;
            }
            catch (InvalidOperationException)
            {
                _logger.LogWarning($"Reader not found: {id}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting reader: {id}");
                throw;
            }
        }

        public async Task<Reader> GetReaderById(int id)
        {
            try
            {
                var reader = await _readerRepository.GetById(id);
                _logger.LogInformation($"Retrieved reader: {reader.Name}");
                return reader;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting reader: {id}");
                throw;
            }
        }

        public async Task<IEnumerable<Reader>> GetAllReaders()
        {
            try
            {
                return await _readerRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all readers");
                throw;
            }
        }
    }
}