using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.Zamowienia
{
    public class UpdateZamowienieDto
    {
        [Required(ErrorMessage = "Numer zamówienia jest wymagany.")]
        [StringLength(100,
            ErrorMessage = "Numer zamówienia może mieć maksymalnie 100 znaków.")]
        public string NumerZamowienia { get; set; } = string.Empty;

        [Range(1, int.MaxValue,
            ErrorMessage = "Nieprawidłowy użytkownik.")]
        public int UzytkownikId { get; set; }

        public int? KlubId { get; set; }

        [Required(ErrorMessage = "Status zamówienia jest wymagany.")]
        [RegularExpression(
            "Nowe|Oczekujące|Opłacone|Anulowane|Zrealizowane",
            ErrorMessage = "Nieprawidłowy status zamówienia.")]
        public string Status { get; set; } = string.Empty;

        [Range(0, 999999,
            ErrorMessage = "Suma brutto nie może być ujemna.")]
        public decimal SumaBrutto { get; set; }

        [Range(0, 999999,
            ErrorMessage = "Kwota rabatu nie może być ujemna.")]
        public decimal KwotaRabatu { get; set; }

        [Required(ErrorMessage = "Waluta jest wymagana.")]
        [StringLength(3, MinimumLength = 3,
            ErrorMessage = "Waluta musi mieć dokładnie 3 znaki.")]
        public string Waluta { get; set; } = "PLN";

        public int? MetodaPlatnosciId { get; set; }

        public int? RabatId { get; set; }

        [Required(ErrorMessage = "Typ kanału jest wymagany.")]
        [RegularExpression(
            "WWW|Desktop|Mobile|Recepcja",
            ErrorMessage = "Nieprawidłowy typ kanału.")]
        public string TypKanalu { get; set; } = string.Empty;

        public DateTime DataUtworzenia { get; set; }

        public DateTime? DataOplacenia { get; set; }

        public int? UtworzylUzytkownikId { get; set; }
    }
}