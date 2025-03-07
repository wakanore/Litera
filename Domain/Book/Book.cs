﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Style { get; set; }
        public string AuthorId { get; set; }
        public DateTime Created { get; set; }
        public List<Reader> Readers { get; set; }
    }
}