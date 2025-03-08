using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application;
using Infrastructure;

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
