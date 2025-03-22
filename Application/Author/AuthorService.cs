using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using Domain;

namespace Application
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public AuthorDto AddAuthor(AuthorDto authorDto)
        {
            var author = new Author
            {
                Name = authorDto.Name,
                Phone = authorDto.Phone
            };

            var createdAuthor = _authorRepository.Add(author);


            return new AuthorDto
            {
                Id = createdAuthor.Id,
                Name = createdAuthor.Name,
                Phone = createdAuthor.Phone
            };
        }

        public bool UpdateAuthor(AuthorDto authorDto)
        {
            if (authorDto == null)
            {
                throw new ArgumentNullException(nameof(authorDto));
            }

            var existingAuthor = _authorRepository.GetById(authorDto.Id);
            if (existingAuthor == null)
            {
                throw new InvalidOperationException("Author not found.");
            }
            existingAuthor.Name = authorDto.Name;
            existingAuthor.Phone = authorDto.Phone;
            _authorRepository.Update(existingAuthor);
            return true;
        }

        public bool DeleteAuthor(int id)
        {
            var authorToDelete = _authorRepository.GetById(id);
            if (authorToDelete == null)
            {
                throw new InvalidOperationException("Author not found.");
            }
            _authorRepository.Delete(id);
            return true;
        }

        public AuthorDto GetAuthorById(int id)
        {
            var author = _authorRepository.GetById(id);
            if (author == null)
            {
                throw new InvalidOperationException("Author not found.");
            }

            return new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                Phone = author.Phone
            };
        }

        public IEnumerable<AuthorDto> GetAllAuthors()
        {
            var authors = _authorRepository.GetAll();
            return authors.Select(author => new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                Phone = author.Phone
            });
        }
    }
}
