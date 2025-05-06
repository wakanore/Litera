using System;
using System.Collections.Generic;
using Infrastructure;

namespace Application
{
    public interface IFavouriteService
    {
        Task<bool> AddFavourite(CreateFavouriteRequest favouriteDto);
        Task<bool> FavouriteExists(int authorId, int readerId);
        Task<bool> DeleteFavourite(int authorId, int readerId);
    }
}