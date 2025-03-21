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

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _favouriteService.GetAllFavourite();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _favouriteService.DeleteFavourite(id);
            return NoContent();
        }
    }
}
