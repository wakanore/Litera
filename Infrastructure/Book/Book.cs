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
    }

    public class BookRepository : IBookRepository
    {

        public void Add(Domain.Book book) { }

        public void Update(Domain.Book book) { }

        public void Delete(int id) { }
    }
}