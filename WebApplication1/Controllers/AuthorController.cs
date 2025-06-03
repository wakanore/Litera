using Microsoft.AspNetCore.Mvc;
using Application;
using Domain;
using Application.Services;
using FluentValidation;

namespace API.Controllers
{
    [ApiController]
    [Route("api/authors")]  
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(
            IAuthorService authorService,
            IValidator<CreateAuthorRequest> validator)
        {
            _authorService = authorService ?? throw new ArgumentNullException(nameof(authorService));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _authorService.GetAuthorById(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var authors = await _authorService.GetAllAuthors();
            return Ok(authors);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAuthorRequest authorDto)
        {
            var result = await _authorService.CreateAuthor(authorDto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAuthorRequest authorDto)
        {
            await _authorService.UpdateAuthor(authorDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _authorService.DeleteAuthor(id)
                ? NoContent()
                : NotFound($"Author with id {id} not found");
        }
    }
}