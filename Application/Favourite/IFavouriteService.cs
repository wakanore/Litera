using System;
using System.Collections.Generic;

namespace Application
{
    public interface IFavouriteService
    {
        void AddFavourite(FavouriteDto favouriteDto);
        void DeleteFavourite(int id);
        IEnumerable<FavouriteDto> GetAllFavourite();
    }
}