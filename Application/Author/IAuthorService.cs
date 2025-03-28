using Domain;
using System;
using System.Collections.Generic;

namespace Application
{
    public interface IAuthorService
    {
        Task<Author> AddAuthor(Author author);
        Task<bool> UpdateAuthorAsync(Author author);
        Task<bool> DeleteAuthorAsync(int id);
        Task<Author> GetAuthorById(int id);
        Task<IEnumerable<Author>> GetAllAuthors();
    }
}
