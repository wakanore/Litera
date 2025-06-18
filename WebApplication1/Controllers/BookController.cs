using Microsoft.AspNetCore.Mvc;
using Application;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[[book]]")]
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;

        public BookController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _bookService.GetBookById(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _bookService.GetAllBooks();
            return Ok(products);
        }

        [HttpPost]
        public IActionResult Create(BookDto bookDto)
        {
            _bookService.AddBook(bookDto);
            return CreatedAtAction(nameof(GetById), new { id = bookDto.Id }, bookDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, BookDto bookDto)
        {
            if (id != bookDto.Id)
                return BadRequest();
            _bookService.UpdateBook(bookDto);
            bool isUpdated = _bookService.UpdateBook(bookDto);

            if (isUpdated)
                return NoContent(); 
            else
                return NotFound(); 
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool isDeleted = _bookService.DeleteBook(id);

            if (isDeleted)
                return NoContent();
            else
                return NotFound();
        }
    }
}
