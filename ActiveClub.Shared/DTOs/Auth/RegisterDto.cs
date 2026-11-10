using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Imię jest wymagane.")]
        public string Imie { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        public string Nazwisko { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Podaj poprawny adres email.")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Podaj poprawny numer telefonu.")]
        public string? Telefon { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [MinLength(6, ErrorMessage = "Hasło musi mieć minimum 6 znaków.")]
        public string Haslo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Powtórzenie hasła jest wymagane.")]
        [Compare(nameof(Haslo), ErrorMessage = "Hasła nie są takie same.")]
        public string PowtorzHaslo { get; set; } = string.Empty;

        [Range(typeof(bool), "true", "true",
            ErrorMessage = "Musisz zaakceptować regulamin.")]
        public bool AkceptujeRegulamin { get; set; }
    }
}