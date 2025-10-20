using Microsoft.EntityFrameworkCore;

using BookLibraryManagement.Data;
using BookLibraryManagement.Models;

namespace BookLibraryManagement.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _db;

        public AuthorRepository(AppDbContext db) => _db = db;

        public async Task<Author> AddAsync(Author author)
        {
            _db.Authors.Add(author);
            await _db.SaveChangesAsync();
            return author;
        }

        public async Task DeleteAsync(int id)
        {
            var a = await _db.Authors.FindAsync(id);
            if (a == null) return;
            _db.Authors.Remove(a);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _db.Authors.Include(a => a.Books).ToListAsync();
        }

        public async Task<Author> GetByIdAsync(int id)
        {
            Author? author = await _db.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.AuthorId == id) ?? throw new InvalidOperationException($"Author with id {id} not found.");
            return author;
        }

        public async Task UpdateAsync(Author author)
        {
            _db.Authors.Update(author);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Authors.AnyAsync(a => a.AuthorId == id);
        }
    }
}
