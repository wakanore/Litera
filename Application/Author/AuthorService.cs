using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Application.Exceptions;
using FluentValidation;
using Infrastructure;

namespace Application
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IValidator<CreateAuthorRequest> _createValidator;
        private readonly IValidator<UpdateAuthorRequest> _updateValidator;

        public AuthorService(
            IAuthorRepository authorRepository,
            IValidator<CreateAuthorRequest> createValidator,
            IValidator<UpdateAuthorRequest> updateValidator)
        {
            _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
            _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
            _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
        }

        public async Task<AuthorResponse> CreateAuthor(CreateAuthorRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new Application.Exceptions.ValidationException(validationResult.Errors);

            var author = new Author
            {
                Name = request.Name,
                Phone = request.Phone
            };

            var createdAuthor = await _authorRepository.Add(author);

            return MapToResponse(createdAuthor);
        }

        public async Task<AuthorResponse> UpdateAuthor(UpdateAuthorRequest request)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new Application.Exceptions.ValidationException(validationResult.Errors); ;

            var existingAuthor = await _authorRepository.GetById(request.Id);
            if (existingAuthor == null)
                throw new NotFoundException($"Author with id {request.Id} not found");

            existingAuthor.Name = request.Name;
            existingAuthor.Phone = request.Phone;

            await _authorRepository.Update(existingAuthor);

            return MapToResponse(existingAuthor);
        }

        public async Task<bool> DeleteAuthor(int id)
        {
            var author = await _authorRepository.GetById(id);
            if (author == null)
                throw new NotFoundException($"Author with ID {id} not found.");

            await _authorRepository.Delete(id);
            return true;
        }

        public async Task<AuthorResponse> GetAuthorById(int id)
        {
            var author = await _authorRepository.GetById(id);
            if (author == null)
                throw new NotFoundException($"Author with ID {id} not found.");

            return MapToResponse(author);
        }

        public async Task<IEnumerable<AuthorResponse>> GetAllAuthors()
        {
            var authors = await _authorRepository.GetAll();
            return authors.Select(MapToResponse);
        }

        private AuthorResponse MapToResponse(Author author)
        {
            return new AuthorResponse(
                Id: author.Id,
                Name: author.Name,
                Phone: author.Phone
            );
        }
    }
}
