using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IAuthorRepository
    {
        void Add(Domain.Author user);
        void Update(Domain.Author user);
        void Delete(int id);
        void GetById(int id);
        void GetAll();
    }

    public class AuthorRepository : IAuthorRepository
    {
        private List<Domain.Author> _authors = new List<Domain.Author>();
        public void Add(Domain.Author author) { }

        public void Update(Domain.Author author) { }

        public void Delete(int id) { }
        public void GetById(int id) { }
        public void GetAll() { }
    }

}