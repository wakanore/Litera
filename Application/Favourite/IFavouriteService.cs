using System;
using System.Collections.Generic;
using Infrastructure;

namespace Application
{
    public interface IFavouriteService
    {
        Task<FavouriteResponse> AddFavourite(CreateFavouriteRequest request);
        Task<bool> FavouriteExists(int userId, int bookId);
        Task<bool> DeleteFavourite(int userId, int bookId);
    }
}