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

        public async Task<bool> DeleteFavourite(int authorId, int readerId)
        {
            try
            {
                var favourite = new Favourite { AuthorId = authorId, ReaderId = readerId };
                await _favouriteRepository.Delete(favourite);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Favourite not found.", ex);
            }
        }
    }
}