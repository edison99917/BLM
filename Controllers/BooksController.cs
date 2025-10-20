using Microsoft.AspNetCore.Mvc;

using BookLibraryManagement.DTOs;
using BookLibraryManagement.Models;
using BookLibraryManagement.Services;

namespace BookLibraryManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;
        private readonly AuthorService _authorService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(BookService bookService, AuthorService authorService, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _authorService = authorService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Genre? genre)
        {
            IEnumerable<Book> books = genre.HasValue
                ? await _bookService.GetByGenreAsync(genre.Value)
                : await _bookService.GetAllAsync();

            var dtos = books.Select(b => new BookReadDto
            {
                BookId = b.BookId,
                Title = b.Title,
                PublicationDate = b.PublicationDate,
                Genre = b.Genre,
                Author = new AuthorReadDto
                {
                    AuthorId = b.AuthorId,
                    FullName = b.Author != null ? $"{b.Author.FirstName} {b.Author.LastName}" : ""
                }
            });

            return Ok(dtos);
        }

        [HttpGet("{id}", Name = "GetBookById")]
        public async Task<IActionResult> GetById(int id)
        {
            var b = await _bookService.GetByIdAsync(id);
            if (b == null) return NotFound(new { message = $"Book with id {id} not found." });

            var dto = new BookReadDto
            {
                BookId = b.BookId,
                Title = b.Title,
                PublicationDate = b.PublicationDate,
                Genre = b.Genre,
                Author = new AuthorReadDto { AuthorId = b.AuthorId, FullName = $"{b.Author.FirstName} {b.Author.LastName}" }
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await _authorService.ExistsAsync(dto.AuthorId))
                return BadRequest(new { message = $"Author with id {dto.AuthorId} does not exist." });

            var book = new Book
            {
                Title = dto.Title,
                PublicationDate = dto.PublicationDate,
                Genre = dto.Genre,
                AuthorId = dto.AuthorId
            };

            var created = await _bookService.CreateAsync(book);

            // fetch author to include in response
            var author = await _authorService.GetByIdAsync(created.AuthorId);

            var readDto = new BookReadDto
            {
                BookId = created.BookId,
                Title = created.Title,
                PublicationDate = created.PublicationDate,
                Genre = created.Genre,
                Author = new AuthorReadDto { AuthorId = author.AuthorId, FullName = $"{author.FirstName} {author.LastName}" }
            };

            _logger.LogInformation("Book created: {Title} (id: {BookId})", created.Title, created.BookId);
            return CreatedAtRoute("GetBookById", new { id = created.BookId }, readDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!await _bookService.ExistsAsync(id)) return NotFound(new { message = $"Book with id {id} not found." });
            if (!await _authorService.ExistsAsync(dto.AuthorId)) return BadRequest(new { message = $"Author with id {dto.AuthorId} does not exist." });

            var book = await _bookService.GetByIdAsync(id);
            book.Title = dto.Title;
            book.PublicationDate = dto.PublicationDate;
            book.Genre = dto.Genre;
            book.AuthorId = dto.AuthorId;

            await _bookService.UpdateAsync(book);
            _logger.LogInformation("Book updated: {BookId}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _bookService.ExistsAsync(id)) return NotFound(new { message = $"Book with id {id} not found." });

            await _bookService.DeleteAsync(id);
            _logger.LogInformation("Book deleted: {BookId}", id);
            return NoContent();
        }
    }
}
