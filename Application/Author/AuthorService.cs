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

        public async Task<CreateAuthorRequest> AddAuthor(Author author)
        {
            var addedAuthor = await _authorRepository.Add(author);

            return new CreateAuthorRequest(
                Id: addedAuthor.Id,
                Name: addedAuthor.Name,
                Phone: "" 
            );
        }

        public async Task<bool> UpdateAuthor(Author author)
        {
            try
            {
                await _authorRepository.Update(author);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAuthor(int id)
        {
            try
            {
                await _authorRepository.Delete(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<CreateAuthorRequest> GetAuthorById(int id)
        {
            var author = await _authorRepository.GetById(id);

            return new CreateAuthorRequest(
                Id: author.Id,
                Name: author.Name,
                Phone: "" 
            );
        }

        public async Task<IEnumerable<Author>> GetAllAuthors()
        {
            return await _authorRepository.GetAll();
        }
    }
}
