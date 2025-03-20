using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Application
{
    public class FavouriteService : IFavouriteService
    {
        private readonly IFavouriteRepository _favouriteRepository;

        public FavouriteService(IFavouriteRepository FavouriteRepository)
        {
            _favouriteRepository = FavouriteRepository;
        }


        public void AddFavourite(FavouriteDto favouriteDto)
        {
            var favourite = new Domain.Favourite
            {
                AuthorId = favouriteDto.AuthorId
            };
            _favouriteRepository.Add(favourite);
        }
        
        public void DeleteFavourite(int id)
        {
            try
            {
                _favouriteRepository.Delete(id);
            }
            catch (Exception ex) 
            {
                throw new InvalidOperationException("Favourite not found.", ex);
            }
        }

        public IEnumerable<FavouriteDto> GetAllFavourite()
        {
            var favourites = _favouriteRepository.GetAll();
            return favourites.Select(favourite => new FavouriteDto
            {
                AuthorId = favourite.AuthorId,
                ReaderId = favourite.ReaderId
            });
        }
    }
}