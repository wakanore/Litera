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
        private readonly ReaderService _readerService;
        private readonly IReaderRepository _readerRepository; // 1. Объявляем поле

        // 2. Добавляем конструктор с внедрением зависимости
        public ReaderController(IReaderRepository readerRepository)
        {
            _readerRepository = readerRepository ?? throw new ArgumentNullException(nameof(readerRepository));
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
        public async Task<IActionResult> Create([FromBody] ReaderDto readerDto)
        {
            // Преобразуем DTO в Domain модель
            var reader = new Reader
            {
                Name = readerDto.Name,
                Phone = readerDto.Phone
                // Другие свойства по необходимости
            };

            var result = await _readerRepository.Add(reader);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ReaderDto readerDto)
        {
            try
            {
                // 1. Проверка соответствия ID в пути и теле запроса
                if (id != readerDto.Id)
                {
                    return BadRequest("ID in URL does not match ID in body");
                }

                // 2. Преобразование DTO в Domain модель
                var reader = new Reader
                {
                    Id = readerDto.Id,
                    Name = readerDto.Name,
                    Phone = readerDto.Phone
                };

                // 3. Вызов сервиса (асинхронная версия)
                bool isUpdated = await _readerRepository.Update(reader);

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