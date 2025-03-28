using System;
using System.Collections.Generic;

namespace Application
{
    public interface IFavouriteService
    {
        bool AddFavourite(FavouriteDto favouriteDto);
        Task<bool> DeleteFavourite(int authorId, int readerId);
    }
}