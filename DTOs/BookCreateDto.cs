using System.ComponentModel.DataAnnotations;

using BookLibraryManagement.Models;

namespace BookLibraryManagement.DTOs
{
    public class BookCreateDto
    {
        [Required, MaxLength(150)]
        public string Title { get; set; } = null!;

        [Required]
        public DateTime PublicationDate { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [Required]
        public int AuthorId { get; set; }
    }
}
