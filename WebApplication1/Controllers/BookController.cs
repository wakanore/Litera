using Microsoft.AspNetCore.Mvc;
using Application;
using Domain;
using System;
using System.Threading.Tasks;
using Infrastructure;
using Application.Services;
using Azure.Core;
using FluentValidation;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        }
        private readonly IValidator<CreateBookRequest> _validator;

        public BookController(IValidator<CreateBookRequest> validator)
        {
            _validator = validator;
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
        public async Task<IActionResult> Create([FromBody] CreateBookRequest bookDto)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(bookDto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }
                var book = new CreateBookRequest(
                    bookDto.Id,
                    bookDto.Name,
                    bookDto.Readers ?? new List<CreateReaderRequest>(),
                    new CreateAuthorRequest(
                        bookDto.Author.Id,
                        bookDto.Author.Name,
                        bookDto.Author.Phone
                    )
                );

                var createdBook = await _bookService.AddBook(book);

                var resultDto = new CreateBookRequest(
                    createdBook.Id,
                    createdBook.Name,
                    createdBook.Readers ?? new List<CreateReaderRequest>(),
                    createdBook.Author ?? new CreateAuthorRequest(0, "", "")
                );

                return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateBookRequest bookDto)
        {
            var validationResult = await _validator.ValidateAsync(bookDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            try
            {
                if (id != bookDto.Id)
                {
                    return BadRequest("ID in URL does not match ID in body");
                }

                var book = new CreateBookRequest(
                    Id: bookDto.Id,
                    Name: bookDto.Name,
                    Readers: new List<CreateReaderRequest>(), // или bookDto.Readers, если есть
                    Author: new CreateAuthorRequest( // или bookDto.Author, если есть
                        bookDto.Author.Id,
                        bookDto.Author.Name,
                        bookDto.Author.Phone
                    )
                );

                var isUpdated = await _bookService.UpdateBook(book);
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
                var isDeleted = await _bookService.DeleteBook(id);
                return isDeleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
