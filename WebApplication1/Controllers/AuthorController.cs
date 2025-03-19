using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[[author]]")]
    public class AuthorController : ControllerBase
    {
        private readonly Application.AuthorService _authorService;

        public AuthorController(Application.AuthorService authorService)
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
        public IActionResult Create(Application.AuthorDto authorDto)
        {
            _authorService.AddAuthor(authorDto);
            return CreatedAtAction(nameof(GetById), new { id = authorDto.Id }, authorDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Application.AuthorDto authorDto)
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
