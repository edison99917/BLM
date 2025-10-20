using Microsoft.EntityFrameworkCore;

using BookLibraryManagement.Data;
using BookLibraryManagement.Models;

namespace BookLibraryManagement.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _db;

        public BookRepository(AppDbContext db) => _db = db;

        public async Task<Book> AddAsync(Book book)
        {
            _db.Books.Add(book);
            await _db.SaveChangesAsync();
            return book;
        }

        public async Task DeleteAsync(int id)
        {
            var b = await _db.Books.FindAsync(id);
            if (b == null) return;
            _db.Books.Remove(b);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _db.Books.Include(b => b.Author).ToListAsync();
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            Book? book = await _db.Books.Include(a => a.Author).FirstOrDefaultAsync(a => a.BookId == id) ?? throw new InvalidOperationException($"Book with id {id} not found.");
            return book;
        }

        public async Task<IEnumerable<Book>> GetByGenreAsync(Genre genre)
        {
            return await _db.Books.Include(b => b.Author).Where(b => b.Genre == genre).ToListAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _db.Books.Update(book);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Books.AnyAsync(b => b.BookId == id);
        }
    }
}
