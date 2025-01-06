using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Book")]
        public int BookId { get; set; }
        public Book Book { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; } // Klucz użytkownika z ASP.NET Identity

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; } // Treść recenzji

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } // Ocena od 1 do 5

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
