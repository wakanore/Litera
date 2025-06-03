using Application.Exceptions;
using Domain;
using FluentValidation;
using Infrastructure;

namespace Application
{
    public class FavouriteService : IFavouriteService
    {
        private readonly IFavouriteRepository _favouriteRepository;
        private readonly IValidator<CreateFavouriteRequest> _createValidator;

        public FavouriteService(
            IFavouriteRepository favouriteRepository,
            IValidator<CreateFavouriteRequest> createValidator)
        {
            _favouriteRepository = favouriteRepository ?? throw new ArgumentNullException(nameof(favouriteRepository));
            _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
        }

        public async Task<FavouriteResponse> AddFavourite(CreateFavouriteRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new Application.Exceptions.ValidationException(validationResult.Errors);

            var favourite = new Favourite
            {
                UserId = request.UserId,
                BookId = request.BookId
            };

            var createdFavourite = await _favouriteRepository.Add(favourite);

            return MapToResponse(createdFavourite);
        }

        public async Task<bool> FavouriteExists(int userId, int bookId)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID", nameof(userId));

            if (bookId <= 0)
                throw new ArgumentException("Invalid book ID", nameof(bookId));

            return await _favouriteRepository.FavouriteExists(userId, bookId);
        }

        public async Task<bool> DeleteFavourite(int userId, int bookId)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID", nameof(userId));

            if (bookId <= 0)
                throw new ArgumentException("Invalid book ID", nameof(bookId));

            var exists = await _favouriteRepository.FavouriteExists(userId, bookId);
            if (!exists)
                throw new NotFoundException($"Favourite for user {userId} and book {bookId} not found.");

            await _favouriteRepository.Delete(userId, bookId);
            return true;
        }

        private FavouriteResponse MapToResponse(Favourite favourite)
        {
            return new FavouriteResponse(
                UserId: favourite.UserId,
                BookId: favourite.BookId
            );
        }
    }
}
