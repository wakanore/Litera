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


        public async Task<bool> AddFavourite(CreateFavouriteRequest favouriteDto)
        {
            try
            {
                // 1. Create domain entity from DTO using proper constructor
                var favourite = new Favourite(
                    AuthorId: favouriteDto.AuthorId,
                    readerId: favouriteDto.ReaderId
                );

                // 2. Pass the domain entity to repository
                await _favouriteRepository.Add(favourite);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> FavouriteExists(int authorId, int readerId)
        {
            return await _favouriteRepository.FavouriteExists(authorId, readerId);
        }

        public Task<bool> DeleteFavourite(int authorId, int readerId)
        {
            try
            {
                return _favouriteRepository.Delete(authorId, readerId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Favourite not found.", ex);
            }
        }
    }
}