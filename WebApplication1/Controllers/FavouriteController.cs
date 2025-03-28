using Microsoft.AspNetCore.Mvc;
using Application;

namespace API.Controllers
{
    [Route("api/[[favourite]]")]
    [ApiController]
    public class FavouriteController : ControllerBase
    {
        private readonly FavouriteService _favouriteService;

        public FavouriteController(FavouriteService favouriteService)
        {
            _favouriteService = favouriteService;
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
    }
}
