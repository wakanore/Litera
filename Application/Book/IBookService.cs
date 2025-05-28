using Domain;
using Infrastructure;
using System;
using System.Collections.Generic;

namespace Application
{
    public interface IBookService
    {
        Task<CreateBookRequest> AddBook(Book bookDto);
        Task<bool> UpdateBook(CreateBookRequest book);
        Task<bool> DeleteBook(int id);
        Task<CreateBookRequest> GetBookById(int id);
        Task<IEnumerable<CreateBookRequest>> GetAllBooks();
    }
}
