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
        private readonly IValidator<CreateFavouriteRequest> _validator;

        public FavouriteController(
            IFavouriteService favouriteService,
            IValidator<CreateFavouriteRequest> validator)
        {
            _favouriteService = favouriteService ?? throw new ArgumentNullException(nameof(favouriteService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        [HttpDelete("{UserId}/{BookId}")]
        public async Task<IActionResult> Delete(int UserId, int BookId)
        {
            try
            {
                bool isDeleted = await _favouriteService.DeleteFavourite(UserId, BookId);
                return isDeleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateFavouriteRequest favouriteDto)
        {
            try
            {
                // Validation
                ValidationResult validationResult = await _validator.ValidateAsync(favouriteDto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new
                    {
                        errors = validationResult.Errors.Select(e => new {
                            property = e.PropertyName,
                            message = e.ErrorMessage
                        })
                    });
                }

                // Check if favorite already exists
                bool alreadyExists = await _favouriteService.FavouriteExists(
                    favouriteDto.UserId,
                    favouriteDto.BookId);

                if (alreadyExists)
                {
                    return Conflict(new { error = "This favorite already exists" });
                }

                bool isAdded = await _favouriteService.AddFavourite(favouriteDto);


                return CreatedAtAction(nameof(Add), new
                {
                    authorId = favouriteDto.UserId,
                    readerId = favouriteDto.BookId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }
    }
}