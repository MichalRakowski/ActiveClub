using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.Trenerzy
{
    public class UpdateTrenerDto
    {
        public int? UzytkownikId { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Imię musi mieć od 2 do 100 znaków.")]
        public string Imie { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Nazwisko musi mieć od 2 do 100 znaków.")]
        public string Nazwisko { get; set; } = string.Empty;

        [StringLength(1000,
            ErrorMessage = "Bio może mieć maksymalnie 1000 znaków.")]
        public string? Bio { get; set; }

        [StringLength(500,
            ErrorMessage = "Opis specjalizacji może mieć maksymalnie 500 znaków.")]
        public string? SpecjalizacjaOpis { get; set; }

        [Range(0, 9999,
            ErrorMessage = "Stawka za godzinę nie może być ujemna.")]
        public decimal? StawkaZaGodzine { get; set; }

        public bool CzyAktywny { get; set; } = true;
    }
}