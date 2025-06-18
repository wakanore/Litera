using Domain;
using Infrastructure;
using System;
using System.Collections.Generic;

namespace Application
{
    public interface IBookService
    {
        Task<BookResponse> CreateBook(CreateBookRequest request);
        Task<BookResponse> UpdateBook(UpdateBookRequest request);
        Task<bool> DeleteBook(int id);
        Task<BookResponse> GetBookById(int id);
        Task<IEnumerable<BookResponse>> GetAllBooks();
    }
}
