using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Identyfikator książki jest wymagany.")]
        [ForeignKey("Book")]
        public int BookId { get; set; }
        [NotMapped]
        public Book Book { get; set; } = null;

        [Required(ErrorMessage = "Identyfikator użytkownika jest wymagany.")]
        [ForeignKey("User")]
        public string? UserId { get; set; } // Klucz użytkownika z ASP.NET Identity

        [Required(ErrorMessage = "Treść recenzji jest wymagana.")]
        [MaxLength(1000, ErrorMessage = "Treść recenzji nie może przekraczać 1000 znaków.")]
        [MinLength(10, ErrorMessage = "Treść recenzji musi zawierać co najmniej 10 znaków.")]
        public string? Content { get; set; } // Treść recenzji

        [Required(ErrorMessage = "Ocena jest wymagana.")]
        [Range(1, 5, ErrorMessage = "Ocena musi być liczbą od 1 do 5.")]
        public int Rating { get; set; } // Ocena od 1 do 5

        [Display(Name = "Data utworzenia")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
