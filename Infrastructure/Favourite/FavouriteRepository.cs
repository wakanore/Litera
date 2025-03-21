using System;
using Domain;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure
{
    public class FavouriteRepository : IFavouriteRepository
    {
        private readonly List<Favourite> _favourite = new List<Favourite>();
        public FavouriteRepository()
        {
            _favourite.Add(new Favourite { AuthorId = 1, ReaderId = 1});
            _favourite.Add(new Favourite { AuthorId = 3, ReaderId = 2 });
            _favourite.Add(new Favourite { AuthorId = 2, ReaderId = 2 });
            _favourite.Add(new Favourite { AuthorId = 1, ReaderId = 3 });
        }
        public void Add(Favourite favourite)
        {
            if (_favourite.Any(b => b.AuthorId == favourite.AuthorId))
            {
                throw new InvalidOperationException("favourite with the same ID already exists.");
            }

            _favourite.Add(favourite);
        }

        public void Delete(int author_id)
        {
            var favouriteToDelete = _favourite.FirstOrDefault(b => b.AuthorId == author_id);
            if (favouriteToDelete == null)
            {
                throw new InvalidOperationException("favourite not found.");
            }

            _favourite.Remove(favouriteToDelete);
        }

        public IEnumerable<Favourite> GetAll()
        {
            return _favourite;
        }

    }
}