using System;
using System.Collections.Generic;

namespace Domain
{
    public class Favourite
    {
        public Favourite(int authorId, int readerId)
        {
            AuthorId = authorId;
            ReaderId = readerId;
        }

        public int AuthorId { get; set; }
        public int ReaderId { get; set; }
    }
}