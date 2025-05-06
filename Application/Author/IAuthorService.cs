using Domain;
using System;
using System.Collections.Generic;

namespace Application
{
    public interface IAuthorService
    {
        Task<CreateAuthorRequest> AddAuthor(Author author);
        Task<bool> UpdateAuthor(Author author);
        Task<bool> DeleteAuthor(int id);
        Task<CreateAuthorRequest> GetAuthorById(int id);
        Task<IEnumerable<Author>> GetAllAuthors();
    }
}
