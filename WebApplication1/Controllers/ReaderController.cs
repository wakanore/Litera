using Microsoft.AspNetCore.Mvc;
using Application;
using FluentValidation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Domain;
using Application.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReaderController : ControllerBase
    {
        private readonly IReaderService _readerService;

        public ReaderController(
            IReaderService readerService,
            IValidator<CreateReaderRequest> validator)
        {
            _readerService = readerService ?? throw new ArgumentNullException(nameof(readerService));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _readerService.GetReaderById(id);
                return result != null ? Ok(result) : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var readers = await _readerService.GetAllReaders();
            return Ok(readers);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReaderRequest readerDto)
        {
            var result = await _readerService.CreateReader(readerDto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateReaderRequest readerDto)
        {
            await _readerService.UpdateReader(readerDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _readerService.DeleteReader(id)
               ? NoContent()
               : NotFound($"Author with id {id} not found");
        }
    }
}