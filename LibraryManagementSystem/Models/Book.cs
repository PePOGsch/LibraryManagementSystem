using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tytuł jest wymagany.")]
        [MaxLength(200, ErrorMessage = "Tytuł nie może przekraczać 200 znaków.")]
        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Autor jest wymagany.")]
        [MaxLength(100, ErrorMessage = "Autor nie może przekraczać 100 znaków.")]
        [Display(Name = "Autor")]
        public string Author { get; set; }

        [RegularExpression(@"^\d{10}(\d{3})?$", ErrorMessage = "ISBN musi być liczbą składającą się z 10 lub 13 cyfr.")]
        public string ISBN { get; set; }

        [MaxLength(50, ErrorMessage = "Gatunek nie może przekraczać 50 znaków.")]
        [Display(Name = "Gatunek")]
        public string Genre { get; set; }

        [MaxLength(1000, ErrorMessage = "Opis nie może przekraczać 1000 znaków.")]
        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Url(ErrorMessage = "Podaj prawidłowy adres URL.")]
        [MaxLength(300, ErrorMessage = "Link wydawcy nie może przekraczać 300 znaków.")]
        [Display(Name = "Strona wydawcy")]
        public string PublisherLink { get; set; } // Opcjonalny link do strony wydawnictwa

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Dostępne kopie muszą być liczbą większą lub równą zero.")]
        [Display(Name = "Ilość")]
        public int AvailableCopies { get; set; } // Liczba dostępnych egzemplarzy

        // Relacja z recenzjami
        [Display(Name = "Recenzje")]
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
