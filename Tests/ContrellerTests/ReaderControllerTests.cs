using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers;
using Application;
using Application.Services;
using Domain;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace UnitTests.API.Controllers
{
    public class ReaderControllerTests
    {
        private readonly Mock<IReaderService> _mockReaderService;
        private readonly Mock<IValidator<CreateReaderRequest>> _mockValidator;
        private readonly ReaderController _controller;

        public ReaderControllerTests()
        {
            _mockReaderService = new Mock<IReaderService>();
            _mockValidator = new Mock<IValidator<CreateReaderRequest>>();
            _controller = new ReaderController(_mockReaderService.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task GetById_WhenReaderExists_ReturnsOkResult()
        {
            // Arrange
            var readerId = 1;
            var expectedReader = new ReaderResponse ( readerId, "Test Reader", "12345678910" );
            _mockReaderService.Setup(s => s.GetReaderById(readerId))
                .ReturnsAsync(expectedReader);

            // Act
            var result = await _controller.GetById(readerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedReader, okResult.Value);
        }

        [Fact]
        public async Task GetById_WhenReaderDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var readerId = 1;
            _mockReaderService.Setup(s => s.GetReaderById(readerId))
                .ReturnsAsync((ReaderResponse)null);

            // Act
            var result = await _controller.GetById(readerId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAll_WhenCalled_ReturnsAllReaders()
        {
            // Arrange
            var expectedReaders = new List<ReaderResponse>
            {
                new ReaderResponse (1, "Reader 1", "12345678910"),
                new ReaderResponse (2, "Reader 2", "12345678911")
            };
            _mockReaderService.Setup(s => s.GetAllReaders())
                .ReturnsAsync(expectedReaders);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var readers = Assert.IsType<List<ReaderResponse>>(okResult.Value);
            Assert.Equal(2, readers.Count);
        }

        [Fact]
        public async Task Create_WithValidReader_ReturnsCreatedAtAction()
        {
            // Arrange
            var createRequest = new CreateReaderRequest (1, "New Reader", "12345678910" );
            var createdReader = new ReaderResponse (1, "New Reader", "12345678912");
            _mockReaderService.Setup(s => s.CreateReader(createRequest))
                .ReturnsAsync(createdReader);

            // Act
            var result = await _controller.Create(createRequest);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(ReaderController.GetById), createdAtActionResult.ActionName);
            Assert.Equal(createdReader.Id, (createdAtActionResult.RouteValues["id"]));
            Assert.Equal(createdReader, createdAtActionResult.Value);
        }

        [Fact]
        public async Task Update_WithValidReader_ReturnsNoContent()
        {
            // Arrange
            var updateRequest = new UpdateReaderRequest (1, "Updated Reader", "12345678910");
            var updatedReader = new ReaderResponse (1, "Updated Reader", "12345678910");
            _mockReaderService.Setup(s => s.UpdateReader(updateRequest))
                .ReturnsAsync(updatedReader);

            // Act
            var result = await _controller.Update(updateRequest.Id, updateRequest);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_WhenReaderExists_ReturnsNoContent()
        {
            // Arrange
            var readerId = 1;
            _mockReaderService.Setup(s => s.DeleteReader(readerId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(readerId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_WhenReaderDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var readerId = 1;
            _mockReaderService.Setup(s => s.DeleteReader(readerId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(readerId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Author with id {readerId} not found", notFoundResult.Value);
        }
    }
}