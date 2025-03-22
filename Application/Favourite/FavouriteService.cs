using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using Domain;

namespace Application
{
    public class FavouriteService : IFavouriteService
    {
        private readonly IFavouriteRepository _favouriteRepository;

        public FavouriteService(IFavouriteRepository favouriteRepository)
        {
            _favouriteRepository = favouriteRepository;
        }


        public bool AddFavourite(FavouriteDto favouriteDto)
        {
            var favourite = new Favourite
            {
                AuthorId = favouriteDto.AuthorId
            };
            _favouriteRepository.Add(favourite);
            return true;
        }
        
        public bool DeleteFavourite(int id)
        {
            try
            {
                _favouriteRepository.Delete(id);
            }
            catch (Exception ex) 
            {
                throw new InvalidOperationException("Favourite not found.", ex);
            }
            return true;
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