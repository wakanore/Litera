using Microsoft.AspNetCore.Mvc;
using Application;
using Domain;
using System;
using System.Threading.Tasks;
using Infrastructure;
using Application.Services;
using FluentValidation;
using System.Collections.Generic;
using static System.Reflection.Metadata.BlobBuilder;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService, IValidator<CreateBookRequest> validator)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
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
            var books = await _bookService.GetAllBooks();
            return Ok(books);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookRequest bookDto)
        {  
            var result = await _bookService.CreateBook(bookDto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);          
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBookRequest bookDto)
        {
            if (id != bookDto.Id)
            {
                return BadRequest("ID in URL does not match ID in body");
            }

            var updatedBook = await _bookService.UpdateBook(bookDto);
            return updatedBook != null ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _bookService.DeleteBook(id);
            return isDeleted ? NoContent() : NotFound();
        }
    }
}