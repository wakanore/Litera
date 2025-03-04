using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private readonly Infrastructure.IAuthorRepository _AuthorRepository;

        public AuthorService(Infrastructure.IAuthorRepository AuthorRepository)
        {
            _AuthorRepository = AuthorRepository;
        }


        public void AddAuthor(AuthorDto AuthorDto)
        {
            var Author = new Domain.Author
            {
                Name = AuthorDto.Name,
                Phone = AuthorDto.Phone
            };
            _AuthorRepository.Add(Author);
        }
    }


    public class ReaderDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }

    public class ReaderService
    {
        private readonly Infrastructure.IReaderRepository _ReaderRepository;

        public ReaderService(Infrastructure.IReaderRepository ReaderRepository)
        {
            _ReaderRepository = ReaderRepository;
        }


        public void AddReader(ReaderDto ReaderDto)
        {
            var Reader = new Domain.Reader
            {
                Name = ReaderDto.Name,
                Phone = ReaderDto.Phone
            };
            _ReaderRepository.Add(Reader);
        }
    }





    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ProductService
    {
        private readonly Infrastructure.IProductRepository _ProductRepository;

        public ProductService(Infrastructure.IProductRepository ProductRepository)
        {
            _ProductRepository = ProductRepository;
        }


        public void AddProduct(ProductDto ProductDto)
        {
            var Product = new Domain.Product
            {
                Name = ProductDto.Name
            };
            _ProductRepository.Add(Product);
        }
    }
}
