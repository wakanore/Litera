using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IBookRepository
    {
        void Add(Domain.Book book);
        void Update(Domain.Book book);
        void Delete(int id);
        Domain.Book GetById(int id);
        IEnumerable<Domain.Book> GetAll();
    }
}