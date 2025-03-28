using System;
using System.Collections.Generic;
using Domain;

namespace Infrastructure
{
    public interface IFavouriteRepository
    {

        Task<Favourite> Add(Favourite favourite);
        Task Delete(Favourite favourite);
    }
}