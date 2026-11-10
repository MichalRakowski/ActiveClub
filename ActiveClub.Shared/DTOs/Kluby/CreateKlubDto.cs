using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.Kluby;

public class CreateKlubDto
{
    [Required(ErrorMessage = "Nazwa klubu jest wymagana.")]
    [StringLength(200, MinimumLength = 2,
        ErrorMessage = "Nazwa klubu musi mieć od 2 do 200 znaków.")]
    public string Nazwa { get; set; } = string.Empty;

    [Required(ErrorMessage = "Miasto jest wymagane.")]
    [StringLength(120, MinimumLength = 2,
        ErrorMessage = "Miasto musi mieć od 2 do 120 znaków.")]
    public string? Miasto { get; set; }

    [Required(ErrorMessage = "Ulica jest wymagana.")]
    [StringLength(200, MinimumLength = 2,
        ErrorMessage = "Ulica musi mieć od 2 do 200 znaków.")]
    public string? Ulica { get; set; }

    [Required(ErrorMessage = "Kod pocztowy jest wymagany.")]
    [StringLength(20, ErrorMessage = "Kod pocztowy może mieć maksymalnie 20 znaków.")]
    [RegularExpression(@"^\d{2}-\d{3}$",
        ErrorMessage = "Kod pocztowy powinien mieć format 00-000.")]
    public string? KodPocztowy { get; set; }

    [Phone(ErrorMessage = "Podaj poprawny numer telefonu.")]
    [StringLength(30, ErrorMessage = "Telefon może mieć maksymalnie 30 znaków.")]
    public string? Telefon { get; set; }

    [EmailAddress(ErrorMessage = "Podaj poprawny adres email.")]
    [StringLength(320, ErrorMessage = "Email może mieć maksymalnie 320 znaków.")]
    public string? EmailKontakt { get; set; }

    [StringLength(1000, ErrorMessage = "Opis może mieć maksymalnie 1000 znaków.")]
    public string? Opis { get; set; }

    public bool CzyAktywny { get; set; } = true;

    public DateTime DataUtworzenia { get; set; } = DateTime.Now;

    public DateTime? DataModyfikacji { get; set; }
}