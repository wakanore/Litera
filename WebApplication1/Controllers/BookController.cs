using Microsoft.AspNetCore.Mvc;
using Application;
using Domain;
using System;
using System.Threading.Tasks;
using Infrastructure;
using Application.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;
        private readonly IBookRepository _bookRepository; // 1. Объявляем поле

        // 2. Добавляем конструктор с внедрением зависимости
        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var bookDto = await _bookService.GetBookById(id);
                return Ok(bookDto);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var books = await _bookService.GetAllBooks();
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookDto bookDto)
        {
            try
            {
                // Преобразуем DTO в Domain модель
                var book = new Book
                {
                    Name = bookDto.Name
                };

                // Добавляем книгу через репозиторий
                var createdBook = await _bookRepository.Add(book);

                // Преобразуем обратно в DTO для ответа
                var resultDto = new BookDto
                {
                    Id = createdBook.Id,
                    Name = createdBook.Name
                };

                return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookDto bookDto)
        {
            try
            {
                if (id != bookDto.Id)
                {
                    return BadRequest("ID in URL does not match ID in body");
                }

                // Преобразование DTO в Domain модель
                var book = new Book
                {
                    Id = bookDto.Id,
                    Name = bookDto.Name
                };

                var isUpdated = await _bookRepository.Update(book);
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
                var isDeleted = await _bookService.DeleteBookAsync(id);
                return isDeleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
