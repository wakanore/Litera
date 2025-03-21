using Microsoft.AspNetCore.Mvc;
using Application;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[[author]]")]
    public class AuthorController : ControllerBase
    {
        private readonly AuthorService _authorService;

        public AuthorController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _authorService.GetAuthorById(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _authorService.GetAllAuthors();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create(AuthorDto authorDto)
        {
            _authorService.AddAuthor(authorDto);
            return CreatedAtAction(nameof(GetById), new { id = authorDto.Id }, authorDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, AuthorDto authorDto)
        {
            if (id != authorDto.Id)
                return BadRequest();
            _authorService.UpdateAuthor(authorDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _authorService.DeleteAuthor(id);
            return NoContent();
        }
    }
}
