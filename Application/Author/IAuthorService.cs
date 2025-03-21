using System;
using System.Collections.Generic;

namespace Application
{
    public interface IAuthorService
    {
        void AddAuthor(AuthorDto authorDto);
        void UpdateAuthor(AuthorDto authorDto);
        void DeleteAuthor(int id);
        AuthorDto GetAuthorById(int id);
        IEnumerable<AuthorDto> GetAllAuthors();
    }
}
