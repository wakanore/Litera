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

        public async Task<Author> AddAuthor(Author author)
        {
            var addedAuthor = await _authorRepository.Add(author);
            return addedAuthor;
        }

        public Task<bool> UpdateAuthor(Author author)
        {
            return _authorRepository.Update(author)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                        return false;
                    return true;
                });
        }

        public Task<bool> DeleteAuthor(int id)
        {
            return _authorRepository.Delete(id)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                        return false;
                    return true;
                });
        }

        public async Task<Author> GetAuthorById(int id)
        {
            var author = await _authorRepository.GetById(id);
            return author;
        }

        public async Task<IEnumerable<Author>> GetAllAuthors()
        {
            return await _authorRepository.GetAll();
        }
    }
}
