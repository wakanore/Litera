using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[[reader]]")]
    public class ReaderController : ControllerBase
    {
        private readonly ReaderService _readerService;

        public ReaderController(ReaderService readerService)
        {
            _readerService = readerService;
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
            var products = _readerService.GetAll();
            return Ok(products);
        }

        [HttpPost]
        public IActionResult Create(ReaderDto readerDto)
        {
            _readerService.AddReader(readerDto);
            return CreatedAtAction(nameof(GetById), new { id = readerDto.Id }, readerDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, ReaderDto readerDto)
        {
            if (id != readerDto.Id)
                return BadRequest();
            _readerService.UpdateReader(readerDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _readerService.DeleteReader(id);
            return NoContent();
        }
    }
}
