using System.ComponentModel.DataAnnotations;

namespace BookLibraryManagement.DTOs
{
    public class AuthorCreateDto
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = null!;
    }
}
