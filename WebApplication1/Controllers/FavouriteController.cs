using Microsoft.AspNetCore.Mvc;
using Application;
using FluentValidation;
using FluentValidation.Results;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouriteController : ControllerBase
    {
        private readonly IFavouriteService _favouriteService;

        public FavouriteController(
            IFavouriteService favouriteService,
            IValidator<CreateFavouriteRequest> validator)
        {
            _favouriteService = favouriteService ?? throw new ArgumentNullException(nameof(favouriteService));
        }

        [HttpDelete("{UserId}/{BookId}")]
        public async Task<IActionResult> Delete(int UserId, int BookId)
        {
            bool isDeleted = await _favouriteService.DeleteFavourite(UserId, BookId);
            return isDeleted ? NoContent() : NotFound();
        }

        [HttpPost]
        public async Task<bool> Add([FromBody] CreateFavouriteRequest favouriteDto)
        {
            var result = await _favouriteService.AddFavourite(favouriteDto);
            return true;
        }
    }
}