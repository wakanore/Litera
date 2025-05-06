using Microsoft.AspNetCore.Mvc;
using Application;
using Domain;
using Application;
using System;
using System.Threading.Tasks;
using Infrastructure;
using Azure.Core;
using FluentValidation;
using static System.Reflection.Metadata.BlobBuilder;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReaderController : ControllerBase
    {
        private readonly IReaderService _readerService;
        public ReaderController(IReaderService readerService)
        {
            _readerService = readerService ?? throw new ArgumentNullException(nameof(readerService));
        }
        private readonly IValidator<CreateReaderRequest> _validator;

        public ReaderController(IValidator<CreateReaderRequest> validator)
        {
            _validator = validator;
        }

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
        public async Task<IActionResult> Create([FromBody] CreateReaderRequest readerDto)
        {
            var validationResult = await _validator.ValidateAsync(readerDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var reader = new CreateReaderRequest(
                Id: readerDto.Id,                      
                Name: readerDto.Name,                  
                Phone: readerDto.Phone,                
                Books: new List<CreateBookRequest>(),  
                Description: string.Empty             
            );

            var result = await _readerService.AddReader(reader);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
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
                    readerDto.Phone,
                    new List<CreateBookRequest>(),
                    string.Empty
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