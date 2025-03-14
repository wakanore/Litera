using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }

    [ApiController]
    [Route("api/[author]")]
    public class AuthorController : ControllerBase
    {
        private readonly Application.AuthorService _authorService;

        public AuthorController(Application.AuthorService authorService)
        {
            _authorService = authorService;
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _authorService.GetAuthorById(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _authorService.GetAllAuthors();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create(Application.AuthorDto authorDto)
        {
            _authorService.AddAuthor(authorDto);
            return CreatedAtAction(nameof(GetById), new { id = authorDto.Id }, authorDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Application.AuthorDto authorDto)
        {
            if (id != authorDto.Id)
                return BadRequest();
            _authorService.UpdateAuthor(authorDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _authorService.DeleteAuthor(id);
            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[book]")]
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

    [ApiController]
    [Route("api/[reader]")]
    public class ReaderController : ControllerBase
    {
        private readonly Application.ReaderService _readerService;

        public ReaderController(Application.ReaderService readerService)
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
        public IActionResult Create(Application.ReaderDto readerDto)
        {
            _readerService.AddReader(readerDto);
            return CreatedAtAction(nameof(GetById), new { id = readerDto.Id }, readerDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Application.ReaderDto readerDto)
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
