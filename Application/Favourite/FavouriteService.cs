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


        public async Task<bool> AddFavourite(FavouriteDto favouriteDto)
        {
            var favourite = new Favourite
            {
                AuthorId = favouriteDto.AuthorId,
                ReaderId = favouriteDto.ReaderId
            };
            await _favouriteRepository.Add(favourite);
            return true;
        }
        public async Task<bool> FavouriteExists(int authorId, int readerId)
        {
            return await _favouriteRepository.FavouriteExists(authorId, readerId);
        }

        public async Task<bool> DeleteFavourite(int authorId, int readerId)
        {
            try
            {
                await _favouriteRepository.Delete(authorId, readerId);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Favourite not found.", ex);
            }
        }
    }
}