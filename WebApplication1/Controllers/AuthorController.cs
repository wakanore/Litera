using Microsoft.AspNetCore.Mvc;
using Application;
using Domain;
using Infrastructure;
using Application.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[[author]]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly IAuthorRepository _authorRepository;

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
        public async Task<IActionResult> Create([FromBody] AuthorDto authorDto)
        {
            var author = new Author
            {
                Name = authorDto.Name,
                Phone = authorDto.Phone
            };

            var result = await _authorRepository.Add(author);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AuthorDto authorDto)
        {
            try
            {
                if (id != authorDto.Id)
                {
                    return BadRequest("ID in URL does not match ID in body");
                }

                var author = new Author
                {
                    Id = authorDto.Id,
                    Name = authorDto.Name,
                    Phone = authorDto.Phone
                };

                bool isUpdated = await _authorService.UpdateAuthorAsync(author);

                return isUpdated ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _authorService.DeleteAuthorAsync(id);

            return deleted
                ? NoContent()
                : NotFound($"Author with id {id} not found");
        }
    }
}
