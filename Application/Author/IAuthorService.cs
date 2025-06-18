using System;
using System.Collections.Generic;

namespace Application
{
    public interface IAuthorService
    {
        AuthorDto AddAuthor(AuthorDto authorDto);
        bool UpdateAuthor(AuthorDto authorDto);
        bool DeleteAuthor(int id);
        AuthorDto GetAuthorById(int id);
        IEnumerable<AuthorDto> GetAllAuthors();
    }
}