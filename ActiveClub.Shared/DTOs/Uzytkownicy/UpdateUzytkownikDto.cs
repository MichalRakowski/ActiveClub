using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.Uzytkownicy
{
    public class UpdateUzytkownikDto
    {
        [Required(ErrorMessage = "Imię jest wymagane.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Imię musi mieć od 2 do 100 znaków.")]
        public string Imie { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Nazwisko musi mieć od 2 do 100 znaków.")]
        public string Nazwisko { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Podaj poprawny adres email.")]
        [StringLength(320, ErrorMessage = "Email może mieć maksymalnie 320 znaków.")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Podaj poprawny numer telefonu.")]
        [StringLength(30, ErrorMessage = "Telefon może mieć maksymalnie 30 znaków.")]
        public string? Telefon { get; set; }

        public bool CzyAktywny { get; set; } = true;

        public int? KlubDomyslnyId { get; set; }

        [StringLength(500, ErrorMessage = "Uwagi mogą mieć maksymalnie 500 znaków.")]
        public string? Uwagi { get; set; }

        [Required(ErrorMessage = "Rola jest wymagana.")]
        [RegularExpression("Klient|Pracownik|Trener|Admin", ErrorMessage = "Nieprawidłowa rola użytkownika.")]
        public string Rola { get; set; } = "Klient";

        public int? KlubId { get; set; }

        public int? StanowiskoId { get; set; }
    }
}