using System.Text.Json.Serialization;

namespace BookLibraryManagement.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [JsonIgnore]
        public List<Book> Books { get; set; } = [];
    }
}
