using Microsoft.AspNetCore.Mvc;
using Application;
using Domain;
using Infrastructure;
using Application.Services;
using Azure.Core;
using FluentValidation;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[[author]]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService ?? throw new ArgumentNullException(nameof(authorService));
        }
        private readonly IValidator<CreateAuthorRequest> _validator;

        public AuthorController(IValidator<CreateAuthorRequest> validator)
        {
            _validator = validator;
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
        public async Task<IActionResult> Create([FromBody] CreateAuthorRequest authorDto)
        {
            var validationResult = await _validator.ValidateAsync(authorDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var author = new Author
            {
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
            {
                return BadRequest(validationResult.Errors);
            }
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

                bool isUpdated = await _authorService.UpdateAuthor(author);

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
            bool deleted = await _authorService.DeleteAuthor(id);

            return deleted
                ? NoContent()
                : NotFound($"Author with id {id} not found");
        }
    }
}
