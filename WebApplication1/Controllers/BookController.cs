using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[[book]]")]
    public class BookController : ControllerBase
    {
        private readonly Application.BookService _bookService;

        public BookController(Application.BookService bookService)
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
        public IActionResult Create(Application.BookDto bookDto)
        {
            _bookService.AddBook(bookDto);
            return CreatedAtAction(nameof(GetById), new { id = bookDto.Id }, bookDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Application.BookDto bookDto)
        {
            if (id != bookDto.Id)
                return BadRequest();
            _bookService.UpdateBook(bookDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _bookService.DeleteBook(id);
            return NoContent();
        }
    }
}
