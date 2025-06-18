using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain;
using FluentValidation;
using Infrastructure;
using Microsoft.Extensions.Logging;

namespace Application
{
    public class ReaderService : IReaderService
    {
        private readonly IReaderRepository _readerRepository;
        private readonly IValidator<CreateReaderRequest> _createValidator;
        private readonly IValidator<UpdateReaderRequest> _updateValidator;
        private readonly ILogger<ReaderService> _logger;

        public ReaderService(
            IReaderRepository readerRepository,
            IValidator<CreateReaderRequest> createValidator,
            IValidator<UpdateReaderRequest> updateValidator,
            ILogger<ReaderService> logger)
        {
            _readerRepository = readerRepository ?? throw new ArgumentNullException(nameof(readerRepository));
            _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
            _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ReaderResponse> CreateReader(CreateReaderRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for CreateReaderRequest: {Errors}", validationResult.Errors);
                throw new Application.Exceptions.ValidationException(validationResult.Errors);
            }

            var reader = new Reader
            {
                Id = request.Id,
                Name = request.Name,
                Phone = request.Phone
            };

            var createdReader = await _readerRepository.Add(reader);
            _logger.LogInformation("Created new reader with ID: {ReaderId}", createdReader.Id);

            return MapToResponse(createdReader);
        }

        public async Task<ReaderResponse> UpdateReader(UpdateReaderRequest request)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for UpdateReaderRequest: {Errors}", validationResult.Errors);
                throw new Application.Exceptions.ValidationException(validationResult.Errors);
            }

            var existingReader = await _readerRepository.GetById(request.Id);
            if (existingReader == null)
            {
                _logger.LogWarning("Reader with ID {ReaderId} not found for update", request.Id);
                throw new NotFoundException($"Reader with id {request.Id} not found");
            }

            existingReader.Name = request.Name;
            existingReader.Phone = request.Phone;

            await _readerRepository.Update(existingReader);
            _logger.LogInformation("Updated reader with ID: {ReaderId}", request.Id);

            return MapToResponse(existingReader);
        }

        public async Task<bool> DeleteReader(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Attempt to delete reader with invalid ID: {ReaderId}", id);
                throw new ArgumentException("Invalid reader ID", nameof(id));
            }

            var existingReader = await _readerRepository.GetById(id);
            if (existingReader == null)
            {
                _logger.LogWarning("Reader with ID {ReaderId} not found for deletion", id);
                throw new NotFoundException($"Reader with id {id} not found");
            }

            await _readerRepository.Delete(id);
            _logger.LogInformation("Deleted reader with ID: {ReaderId}", id);
            return true;
        }

        public async Task<ReaderResponse> GetReaderById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Attempt to get reader with invalid ID: {ReaderId}", id);
                throw new ArgumentException("Invalid reader ID", nameof(id));
            }

            var reader = await _readerRepository.GetById(id);
            if (reader == null)
            {
                _logger.LogWarning("Reader with ID {ReaderId} not found", id);
                throw new NotFoundException($"Reader with ID {id} not found.");
            }

            return MapToResponse(reader);
        }

        public async Task<IEnumerable<ReaderResponse>> GetAllReaders()
        {
            var readers = await _readerRepository.GetAll();
            _logger.LogInformation("Retrieved {Count} readers", readers.Count());

            return readers.Select(MapToResponse);
        }

        private ReaderResponse MapToResponse(Reader reader)
        {
            return new ReaderResponse(
                Id: reader.Id,
                Name: reader.Name,
                Phone: reader.Phone
            );
        }
    }
}
