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
    }

    public class AuthorRepository : IAuthorRepository
    {

        public void Add(Domain.Author user) { }

        public void Update(Domain.Author user) { }

        public void Delete(int id) { }
    }



    public interface IReaderRepository
    {
        void Add(Domain.Reader user);  
        void Update(Domain.Reader user); 
        void Delete(int id);   
    }

    public class ReaderRepository : IReaderRepository
    {

        public void Add(Domain.Reader user) { }

        public void Update(Domain.Reader user) { }

        public void Delete(int id) { }
    }



    public interface IProductRepository
    {
        void Add(Domain.Product user);   
        void Update(Domain.Product user); 
        void Delete(int id);    
    }

    public class ProductRepository : IProductRepository
    {

        public void Add(Domain.Product user) { }

        public void Update(Domain.Product user) { }

        public void Delete(int id) { }
    }
}
