using Microsoft.AspNetCore.Mvc;
using Application;
using FluentValidation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Domain;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReaderController : ControllerBase
    {
        private readonly IReaderService _readerService;
        private readonly IValidator<CreateReaderRequest> _validator;

        // Единственный конструктор, объединяющий обе зависимости
        public ReaderController(
            IReaderService readerService,
            IValidator<CreateReaderRequest> validator)
        {
            _readerService = readerService ?? throw new ArgumentNullException(nameof(readerService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
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
            try
            {
                var result = await _readerService.GetAllReaders();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReaderRequest readerDto)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(readerDto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                var reader = new Reader
                {
                    Id = readerDto.Id,
                    Name = readerDto.Name,
                    Phone = readerDto.Phone
                };


                var result = await _readerService.AddReader(reader);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateReaderRequest readerDto)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(readerDto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                if (id != readerDto.Id)
                {
                    return BadRequest("ID in URL does not match ID in body");
                }

                var reader = new CreateReaderRequest(
                    readerDto.Id,
                    readerDto.Name,
                    readerDto.Phone
                );

                bool isUpdated = await _readerService.UpdateReader(reader);
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
                var isDeleted = await _readerService.DeleteReader(id);
                return isDeleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}