using System;
using System.Collections.Generic;

namespace Application
{
    public interface IFavouriteService
    {
        bool AddFavourite(FavouriteDto favouriteDto);
        bool DeleteFavourite(int id);
        IEnumerable<FavouriteDto> GetAllFavourite();
    }
}