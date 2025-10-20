using BookLibraryManagement.Models;
using BookLibraryManagement.Repositories;

namespace BookLibraryManagement.Services
{
    public class BookService
    {
        private readonly IBookRepository _repo;

        public BookService(IBookRepository repo) => _repo = repo;

        public Task<IEnumerable<Book>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Book> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<IEnumerable<Book>> GetByGenreAsync(Genre genre) => _repo.GetByGenreAsync(genre);
        public Task<Book> CreateAsync(Book b) => _repo.AddAsync(b);
        public Task UpdateAsync(Book b) => _repo.UpdateAsync(b);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
        public Task<bool> ExistsAsync(int id) => _repo.ExistsAsync(id);
    }
}
