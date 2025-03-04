using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public List<Product> Products { get; set; }
    }


    public class Reader
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public List<Product> Products { get; set; }
    }


    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Style { get; set; }
        public string Id_author { get; set; }
        public DateTime Date { get; set; }
        public List<Reader> Readers { get; set; }
    }
}
