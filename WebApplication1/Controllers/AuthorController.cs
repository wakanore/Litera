
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
        private readonly IValidator<CreateAuthorRequest> _validator;

        public AuthorController(
            IAuthorService authorService,
            IValidator<CreateAuthorRequest> validator)
        {
            _authorService = authorService ?? throw new ArgumentNullException(nameof(authorService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _authorService.GetAuthorById(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var authors = await _authorService.GetAllAuthors();
                return Ok(authors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAuthorRequest authorDto)
        {
            var validationResult = await _validator.ValidateAsync(authorDto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var author = new Author
            {
                Id = authorDto.Id,
                Name = authorDto.Name,
                Phone = authorDto.Phone
            };

            var result = await _authorService.AddAuthor(author);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateAuthorRequest authorDto)
        {
            var validationResult = await _validator.ValidateAsync(authorDto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            if (id != authorDto.Id)
                return BadRequest("ID in URL does not match ID in body");

            var author = new Author
            {
                Id = authorDto.Id,
                Name = authorDto.Name,
                Phone = authorDto.Phone
            };

            return await _authorService.UpdateAuthor(author)
                ? NoContent()
                : NotFound();
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