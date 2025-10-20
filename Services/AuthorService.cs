using BookLibraryManagement.Models;
using BookLibraryManagement.Repositories;

namespace BookLibraryManagement.Services
{
    public class AuthorService
    {
        private readonly IAuthorRepository _repo;

        public AuthorService(IAuthorRepository repo) => _repo = repo;

        public Task<IEnumerable<Author>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Author> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<Author> CreateAsync(Author a) => _repo.AddAsync(a);
        public Task UpdateAsync(Author a) => _repo.UpdateAsync(a);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
        public Task<bool> ExistsAsync(int id) => _repo.ExistsAsync(id);
    }
}
