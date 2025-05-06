using Microsoft.AspNetCore.Mvc;
using Application;
using Azure.Core;
using FluentValidation;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouriteController : ControllerBase
    {
        private readonly IFavouriteService _favouriteService;
        public FavouriteController(IFavouriteService favouriteService)
        {
            _favouriteService = favouriteService ?? throw new ArgumentNullException(nameof(favouriteService));
        }
        private readonly IValidator<CreateFavouriteRequest> _validator;

        public FavouriteController(IValidator<CreateFavouriteRequest> validator)
        {
            _validator = validator;
        }

        [HttpDelete("{authorId}/{readerId}")]
        public async Task<IActionResult> Delete(int authorId, int readerId)
        {
            try
            {
                bool isDeleted = await _favouriteService.DeleteFavourite(authorId, readerId);
                return isDeleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateFavouriteRequest favouriteDto)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(favouriteDto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }
                bool alreadyExists = await _favouriteService.FavouriteExists(favouriteDto.AuthorId, favouriteDto.ReaderId);
                if (alreadyExists)
                {
                    return Conflict("This favorite already exists");
                }

                bool isAdded = await _favouriteService.AddFavourite(favouriteDto);

                return CreatedAtRoute(new
                {
                    authorId = favouriteDto.AuthorId,
                    readerId = favouriteDto.ReaderId
                }, null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
