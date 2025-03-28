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

        // Все методы делаем асинхронными
        public async Task<Author> AddAuthor(Author author)
        {
            var addedAuthor = await _authorRepository.Add(author);
            // Теперь можно обращаться к свойствам:
            Console.WriteLine($"Added author: {addedAuthor.Name}");
            return addedAuthor;
        }

        public async Task<bool> UpdateAuthorAsync(Author author)
        {
            try
            {
                await _authorRepository.Update(author);
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAuthorAsync(int id)
        {
            try
            {
                await _authorRepository.Delete(id);
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        public async Task<Author> GetAuthorById(int id)
        {
            var author = await _authorRepository.GetById(id);
            Console.WriteLine($"Retrieved author: {author.Name}");
            return author;
        }

        public async Task<IEnumerable<Author>> GetAllAuthors()
        {
            return await _authorRepository.GetAll();
        }
    }
}
