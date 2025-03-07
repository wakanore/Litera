using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Application
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }

    public class AuthorService
    {
        private readonly Infrastructure.IAuthorRepository _authorRepository;

        public AuthorService(Infrastructure.IAuthorRepository AuthorRepository)
        {
            _authorRepository = AuthorRepository;
        }


        public void AddAuthor(AuthorDto AuthorDto)
        {
            var Author = new Domain.Author
            {
                Name = AuthorDto.Name,
                Phone = AuthorDto.Phone
            };
            _authorRepository.Add(Author);
        }
    }

    
}
