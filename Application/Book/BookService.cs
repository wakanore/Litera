using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain;
using FluentValidation;
using Infrastructure;

namespace Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IValidator<CreateBookRequest> _createValidator;
        private readonly IValidator<UpdateBookRequest> _updateValidator;

        public BookService(
            IBookRepository bookRepository,
            IValidator<CreateBookRequest> createValidator = null,
            IValidator<UpdateBookRequest> updateValidator = null)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<BookResponse> CreateBook(CreateBookRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (_createValidator != null)
            {
                var validationResult = await _createValidator.ValidateAsync(request);
                if (!validationResult.IsValid)
                    throw new Application.Exceptions.ValidationException(validationResult.Errors);
            }

            var book = new Book
            {
                Name = request.Name,
                Style = request.Style,
                AuthorId = request.AuthorId
            };

            var createdBook = await _bookRepository.Add(book);
            return MapToResponse(createdBook);
        }

        public async Task<BookResponse> UpdateBook(UpdateBookRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new Application.Exceptions.ValidationException(validationResult.Errors);

            var existingBook = await _bookRepository.GetById(request.Id);
            if (existingBook == null)
                throw new NotFoundException($"Book with id {request.Id} not found");

            existingBook.Name = request.Name;
            existingBook.Style = request.Style;
            existingBook.AuthorId = request.AuthorId;

            await _bookRepository.Update(existingBook);
            return MapToResponse(existingBook);
        }

        public async Task<bool> DeleteBook(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID must be positive", nameof(id));

            var book = await _bookRepository.GetById(id);
            if (book == null)
                throw new NotFoundException($"Book with ID {id} not found");

            await _bookRepository.Delete(id);
            return true;
        }

        public async Task<BookResponse> GetBookById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID must be positive", nameof(id));

            var book = await _bookRepository.GetById(id);
            if (book == null)
                throw new NotFoundException($"Book with ID {id} not found");

            return MapToResponse(book);
        }

        public async Task<IEnumerable<BookResponse>> GetAllBooks()
        {
            var books = await _bookRepository.GetAll();
            return books.Select(MapToResponse).ToList();
        }

        private static BookResponse MapToResponse(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            return new BookResponse(
                Id: book.Id,
                Name: book.Name,
                Style: book.Style,
                AuthorId: book.AuthorId
            );
        }
    }
}

