using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class Loan
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
        public DateTime LoanDate { get; set; } // Data wypożyczenia

        [Required]
        public DateTime ReturnDate { get; set; } // Termin zwrotu

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } // Status: "Wypożyczona", "Zwrócona", "Przedłużona"
    }
}
