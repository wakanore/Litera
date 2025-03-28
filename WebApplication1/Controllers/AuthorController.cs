using Microsoft.AspNetCore.Mvc;
using Application;
using Domain;
using Infrastructure;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[[author]]")]
    public class AuthorController : ControllerBase
    {
        private readonly AuthorService _authorService;
        private readonly IAuthorRepository _authorRepository; // 1. Объявляем поле

        // 2. Добавляем конструктор с внедрением зависимости
        public AuthorController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
        }


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
        public async Task<IActionResult> Create([FromBody] AuthorDto authorDto)
        {
            // Преобразуем DTO в Domain модель
            var author = new Author
            {
                Name = authorDto.Name,
                Phone = authorDto.Phone
                // Другие свойства по необходимости
            };

            var result = await _authorRepository.Add(author);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AuthorDto authorDto)
        {
            try
            {
                // 1. Проверка соответствия ID в пути и теле запроса
                if (id != authorDto.Id)
                {
                    return BadRequest("ID in URL does not match ID in body");
                }

                // 2. Преобразование DTO в Domain модель
                var author = new Author
                {
                    Id = authorDto.Id,
                    Name = authorDto.Name,
                    Phone = authorDto.Phone
                };

                // 3. Вызов сервиса (асинхронная версия)
                bool isUpdated = await _authorService.UpdateAuthorAsync(author);

                // 4. Возврат результата
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
            try
            {
                bool deleted = await _authorService.DeleteAuthorAsync(id);

                return deleted
                    ? NoContent()
                    : NotFound($"Author with id {id} not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
