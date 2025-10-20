using BookLibraryManagement.Models;

namespace BookLibraryManagement.DTOs
{
    public class BookReadDto
    {
        public int BookId { get; set; }

        public string? Title { get; set; } = null!;

        public DateTime PublicationDate { get; set; }

        public Genre Genre { get; set; }

        public AuthorReadDto Author { get; set; } = null!;
    }
}
