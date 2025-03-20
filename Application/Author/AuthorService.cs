using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application;
using Infrastructure;
using Domain;

namespace Application
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository AuthorRepository)
        {
            _authorRepository = AuthorRepository;
        }

        public void AddAuthor(AuthorDto authorDto)
        {
            var Author = new Author
            {
                Name = AuthorDto.Name,
                Phone = AuthorDto.Phone
            };
            _authorRepository.Add(Author);
        }

        public void UpdateAuthor(AuthorDto authorDto)
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
        }

        public void DeleteAuthor(int id)
        {
            var authorToDelete = _authorRepository.GetById(id);
            if (authorToDelete == null)
            {
                throw new InvalidOperationException("Author not found.");
            }
            _authorRepository.Delete(id);
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
