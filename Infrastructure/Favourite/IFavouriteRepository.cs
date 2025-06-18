using System;
using System.Collections.Generic;
using Domain;

namespace Infrastructure
{
    public interface IFavouriteRepository
    {
        void Add(Favourite book);
        void Delete(int id);
        IEnumerable<Favourite> GetAll();
    }
}