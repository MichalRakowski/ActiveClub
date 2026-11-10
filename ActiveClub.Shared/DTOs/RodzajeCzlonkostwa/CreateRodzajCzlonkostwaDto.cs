using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.RodzajeCzlonkostwa;

public class CreateRodzajCzlonkostwaDto
{
    [Required(ErrorMessage = "Kod karnetu jest wymagany.")]
    [StringLength(50, MinimumLength = 2,
        ErrorMessage = "Kod musi mieć od 2 do 50 znaków.")]
    public string Kod { get; set; } = "";

    [Required(ErrorMessage = "Nazwa karnetu jest wymagana.")]
    [StringLength(100, MinimumLength = 2,
        ErrorMessage = "Nazwa musi mieć od 2 do 100 znaków.")]
    public string Nazwa { get; set; } = "";

    [StringLength(1000,
        ErrorMessage = "Opis może mieć maksymalnie 1000 znaków.")]
    public string? Opis { get; set; }

    [Range(0.01, 99999,
        ErrorMessage = "Cena musi być większa od 0.")]
    public decimal? Cena { get; set; }

    [Required(ErrorMessage = "Typ rozliczenia jest wymagany.")]
    [RegularExpression("Okresowe|NaWejścia|Jednorazowe",
        ErrorMessage = "Nieprawidłowy typ rozliczenia.")]
    public string TypRozliczenia { get; set; } = "";

    [Range(1, 3650,
        ErrorMessage = "Okres dni musi być większy od 0.")]
    public int? OkresDni { get; set; }

    [Range(1, 1000,
        ErrorMessage = "Liczba wejść musi być większa od 0.")]
    public int? LiczbaWejsc { get; set; }

    public bool CzyAktywny { get; set; }

    public bool CzyWidoczneOnline { get; set; }
}