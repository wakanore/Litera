using Microsoft.AspNetCore.Mvc;
using Application;
using Domain;
using System;
using System.Threading.Tasks;
using Infrastructure;
using Application.Services;
using FluentValidation;
using System.Collections.Generic;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IValidator<CreateBookRequest> _validator;

        public BookController(IBookService bookService, IValidator<CreateBookRequest> validator)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
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
                var createdBook = new Book
                {
                    Id = bookDto.Id,
                    Name = bookDto.Name,
                    AuthorId = bookDto.AuthorId
                };
                var result = await _bookService.AddBook(createdBook);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateBookRequest bookDto)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(bookDto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                if (id != bookDto.Id)
                {
                    return BadRequest("ID in URL does not match ID in body");
                }

                var isUpdated = await _bookService.UpdateBook(bookDto);
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