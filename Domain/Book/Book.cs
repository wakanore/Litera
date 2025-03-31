using System;
using System.Collections.Generic;

namespace Domain
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Style { get; set; }
        public int AuthorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LinkToCover { get; set; }
    }
}