using Domain;
using System;
using System.Collections.Generic;

namespace Application
{
    public interface IAuthorService
    {
        Task<AuthorResponse> CreateAuthor(CreateAuthorRequest request);
        Task<AuthorResponse> UpdateAuthor(UpdateAuthorRequest request);
        Task<bool> DeleteAuthor(int id);
        Task<AuthorResponse> GetAuthorById(int id);
        Task<IEnumerable<AuthorResponse>> GetAllAuthors();
    }
}
