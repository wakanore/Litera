using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Exceptions;
using Domain;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Application.Tests
{
    public class ReaderServiceTests
    {
        private readonly Mock<IReaderRepository> _mockRepository;
        private readonly Mock<IValidator<CreateReaderRequest>> _mockCreateValidator;
        private readonly Mock<IValidator<UpdateReaderRequest>> _mockUpdateValidator;
        private readonly Mock<ILogger<ReaderService>> _mockLogger;
        private readonly ReaderService _service;

        public ReaderServiceTests()
        {
            _mockRepository = new Mock<IReaderRepository>();
            _mockCreateValidator = new Mock<IValidator<CreateReaderRequest>>();
            _mockUpdateValidator = new Mock<IValidator<UpdateReaderRequest>>();
            _mockLogger = new Mock<ILogger<ReaderService>>();

            _service = new ReaderService(
                _mockRepository.Object,
                _mockCreateValidator.Object,
                _mockUpdateValidator.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task CreateReader_ValidRequest_ReturnsReaderResponse()
        {
            // Arrange
            var request = new CreateReaderRequest ( Id : 1, Name : "John Doe", Phone : "1234567890" );
            var reader = new Reader { Id = 1, Name = request.Name, Phone = request.Phone };

            _mockCreateValidator
                .Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult());

            _mockRepository
                .Setup(r => r.Add(It.IsAny<Reader>()))
                .ReturnsAsync(reader);

            // Act
            var result = await _service.CreateReader(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(reader.Id, result.Id);
            Assert.Equal(reader.Name, result.Name);
            Assert.Equal(reader.Phone, result.Phone);

            _mockRepository.Verify(r => r.Add(It.IsAny<Reader>()), Times.Once);
            _mockLogger.Verify(
                x => x.LogInformation("Created new reader with ID: {ReaderId}", reader.Id),
                Times.Once);
        }

        [Fact]
        public async Task CreateReader_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var request = new CreateReaderRequest(Id: 1, Name: "John Doe", Phone: "1234567890");
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Name is required"),
                new ValidationFailure("Phone", "Phone is required")
            };

            _mockCreateValidator
                .Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult(failures));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Application.Exceptions.ValidationException>(
        () => _service.CreateReader(request));

            Assert.Equal(2, exception.Errors.Count);
            _mockRepository.Verify(r => r.Add(It.IsAny<Reader>()), Times.Never);
            _mockLogger.Verify(
                x => x.LogWarning("Validation failed for CreateReaderRequest: {Errors}", failures),
                Times.Once);
        }

        [Fact]
        public async Task UpdateReader_ValidRequest_UpdatesReader()
        {
            // Arrange
            var request = new UpdateReaderRequest ( Id : 1, Name : "Updated Name", Phone : "9876543210" );
            var existingReader = new Reader { Id = 1, Name = "Old Name", Phone = "1234567890" };

            _mockUpdateValidator
                .Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult());

            _mockRepository
                .Setup(r => r.GetById(request.Id))
                .ReturnsAsync(existingReader);

            _mockRepository
                .Setup(r => r.Update(It.IsAny<Reader>()))
                .ReturnsAsync(true);

            // Act
            var result = await _service.UpdateReader(request);

            // Assert
            Assert.Equal(request.Id, result.Id);
            Assert.Equal(request.Name, result.Name);
            Assert.Equal(request.Phone, result.Phone);

            _mockRepository.Verify(r => r.Update(existingReader), Times.Once);
            _mockLogger.Verify(
                x => x.LogInformation("Updated reader with ID: {ReaderId}", request.Id),
                Times.Once);
        }

        [Fact]
        public async Task UpdateReader_ReaderNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var request = new UpdateReaderRequest(Id: 1, Name: "John Doe", Phone: "1234567890");

            _mockUpdateValidator
                .Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult());

            _mockRepository
                .Setup(r => r.GetById(request.Id))
                .ReturnsAsync((Reader)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateReader(request));
            _mockLogger.Verify(
                x => x.LogWarning("Reader with ID {ReaderId} not found for update", request.Id),
                Times.Once);
        }

        [Fact]
        public async Task DeleteReader_ValidId_ReturnsTrue()
        {
            // Arrange
            int id = 1;
            var reader = new Reader { Id = id };

            _mockRepository
                .Setup(r => r.GetById(id))
                .ReturnsAsync(reader);

            _mockRepository
                .Setup(r => r.Delete(id))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteReader(id);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(r => r.Delete(id), Times.Once);
            _mockLogger.Verify(
                x => x.LogInformation("Deleted reader with ID: {ReaderId}", id),
                Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task DeleteReader_InvalidId_ThrowsArgumentException(int id)
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.DeleteReader(id));
            _mockLogger.Verify(
                x => x.LogWarning("Attempt to delete reader with invalid ID: {ReaderId}", id),
                Times.Once);
        }

        [Fact]
        public async Task GetReaderById_ValidId_ReturnsReaderResponse()
        {
            // Arrange
            int id = 1;
            var reader = new Reader { Id = id, Name = "Test Reader", Phone = "1234567890" };

            _mockRepository
                .Setup(r => r.GetById(id))
                .ReturnsAsync(reader);

            // Act
            var result = await _service.GetReaderById(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(reader.Id, result.Id);
            Assert.Equal(reader.Name, result.Name);
            Assert.Equal(reader.Phone, result.Phone);
        }

        [Fact]
        public async Task GetReaderById_ReaderNotFound_ThrowsNotFoundException()
        {
            // Arrange
            int id = 1;

            _mockRepository
                .Setup(r => r.GetById(id))
                .ReturnsAsync((Reader)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetReaderById(id));
            _mockLogger.Verify(
                x => x.LogWarning("Reader with ID {ReaderId} not found", id),
                Times.Once);
        }

        [Fact]
        public async Task GetAllReaders_ReturnsAllReaders()
        {
            // Arrange
            var readers = new List<Reader>
            {
                new Reader { Id = 1, Name = "Reader 1", Phone = "1111111111" },
                new Reader { Id = 2, Name = "Reader 2", Phone = "2222222222" }
            };

            _mockRepository
                .Setup(r => r.GetAll())
                .ReturnsAsync(readers);

            // Act
            var result = await _service.GetAllReaders();

            // Assert
            Assert.Equal(2, result.Count());
            _mockLogger.Verify(
                x => x.LogInformation("Retrieved {Count} readers", readers.Count),
                Times.Once);
        }
    }
}