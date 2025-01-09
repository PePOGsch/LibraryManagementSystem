using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models
{
    public class Reservation
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
        public DateTime ReservationDate { get; set; } = DateTime.Now;

        public DateTime? ExpirationDate { get; set; } // Data wygaśnięcia rezerwacji (opcjonalne)

        [Required]
        public string Status { get; set; } = "Oczekuje"; // Status: Oczekuje, Zakończony, Anulowany
    }
}

