namespace BookLibraryManagement.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string? Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public Genre Genre { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;
    }
}
