using Microsoft.AspNetCore.Mvc;

using BookLibraryManagement.DTOs;
using BookLibraryManagement.Models;
using BookLibraryManagement.Services;

namespace BookLibraryManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly AuthorService _service;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(AuthorService service, ILogger<AuthorsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var authors = await _service.GetAllAsync();
            var dtos = authors.Select(a => new AuthorReadDto
            {
                AuthorId = a.AuthorId,
                FullName = $"{a.FirstName} {a.LastName}"
            });
            return Ok(dtos);
        }

        [HttpGet("{id}", Name = "GetAuthorById")]
        public async Task<IActionResult> GetById(int id)
        {
            var a = await _service.GetByIdAsync(id);
            if (a == null) return NotFound(new { message = $"Author with id {id} not found." });

            var dto = new AuthorReadDto
            {
                AuthorId = a.AuthorId,
                FullName = $"{a.FirstName} {a.LastName}"
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AuthorCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var author = new Author
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            var created = await _service.CreateAsync(author);
            _logger.LogInformation("Author created: {AuthorName} (id: {AuthorId})", created.FirstName + " " + created.LastName, created.AuthorId);

            var readDto = new AuthorReadDto { AuthorId = created.AuthorId, FullName = $"{created.FirstName} {created.LastName}" };
            return CreatedAtRoute("GetAuthorById", new { id = created.AuthorId }, readDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AuthorCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!await _service.ExistsAsync(id)) return NotFound(new { message = $"Author with id {id} not found." });

            var author = await _service.GetByIdAsync(id);
            author.FirstName = dto.FirstName;
            author.LastName = dto.LastName;

            await _service.UpdateAsync(author);
            _logger.LogInformation("Author updated: {AuthorId}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _service.ExistsAsync(id)) return NotFound(new { message = $"Author with id {id} not found." });

            await _service.DeleteAsync(id);
            _logger.LogInformation("Author deleted: {AuthorId}", id);
            return NoContent();
        }
    }
}
