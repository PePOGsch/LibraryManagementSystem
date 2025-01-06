using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [MaxLength(100)]
        public string Author { get; set; }

        [MaxLength(13)]
        public string ISBN { get; set; }

        [MaxLength(50)]
        public string Genre { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [MaxLength(300)]
        public string PublisherLink { get; set; } // Opcjonalny link do strony wydawnictwa

        [Required]
        public int AvailableCopies { get; set; } // Liczba dostępnych egzemplarzy

        // Relacja z recenzjami
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
