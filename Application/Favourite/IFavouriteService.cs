using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Application
{
    public interface IFavouriteService
    {
        void AddFavourite(FavouriteDto favouriteDto);
        void DeleteFavourite(int id);
        IEnumerable<FavouriteDto> GetAllFavourite();
    }
}