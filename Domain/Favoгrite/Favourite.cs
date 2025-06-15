using System;
using System.Collections.Generic;

namespace Domain
{
    public class Favourite
    {
        //public Favourite(int userId, int bookId)
        //{
        //    UserId = userId;
        //    BookId = bookId;
        //}

        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}