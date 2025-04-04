using Microsoft.AspNetCore.Mvc;
using Application;
using Domain;
using Application;
using System;
using System.Threading.Tasks;
using Infrastructure;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReaderController : ControllerBase
    {
        private readonly IReaderService _readerService;

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _readerService.GetReaderById(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _readerService.GetAllReaders();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReaderDto readerDto)
        {
            var reader = new ReaderDto
            {
                Name = readerDto.Name,
                Phone = readerDto.Phone
            };

            var result = await _readerService.AddReader(reader);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ReaderDto readerDto)
        {
            try
            {
                if (id != readerDto.Id)
                {
                    return BadRequest("ID in URL does not match ID in body");
                }

                var reader = new ReaderDto
                {
                    Id = readerDto.Id,
                    Name = readerDto.Name,
                    Phone = readerDto.Phone
                };

                bool isUpdated = await _readerService.UpdateReaderAsync(reader);

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
                var isDeleted = await _readerService.DeleteReaderAsync(id);
                return isDeleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}