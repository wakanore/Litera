using Domain;
using System;
using System.Collections.Generic;

namespace Application
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Reader> Readers { get; set; }
        public Author Author { get; set; }
    }
}