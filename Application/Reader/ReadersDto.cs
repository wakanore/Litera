﻿using Domain;
using System;
using System.Collections.Generic;

namespace Application
{
    public class ReaderDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public List<BookDto> Books { get; set; }
        public string Description { get; set; }
    }
}