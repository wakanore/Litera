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
        Domain.Author GetById(int id);
        IEnumerable<Domain.Author> GetAll();
    }
}