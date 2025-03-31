using System;
using System.Collections.Generic;
using Domain;

namespace Infrastructure
{
    public interface IFavouriteRepository
    {
        Task<bool> FavouriteExists(int authorId, int readerId);
        Task<Favourite> Add(Favourite favourite);
        Task<bool> Delete(int authorId, int readerId);
    }
}