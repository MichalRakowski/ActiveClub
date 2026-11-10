using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.Rabaty
{
    public class CreateRabatDto
    {
        [Required(ErrorMessage = "Kod rabatu jest wymagany.")]
        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "Kod musi mieć od 2 do 50 znaków.")]
        public string Kod { get; set; } = string.Empty;

        [Required(ErrorMessage = "Typ rabatu jest wymagany.")]
        [RegularExpression(
            "PROCENTOWY|KWOTOWY",
            ErrorMessage = "Nieprawidłowy typ rabatu.")]
        public string Typ { get; set; } = string.Empty;

        [Range(0.01, 999999,
            ErrorMessage = "Wartość rabatu musi być większa od 0.")]
        public decimal Wartosc { get; set; }

        public DateOnly? DataOd { get; set; }

        public DateOnly? DataDo { get; set; }

        [Range(0, 999999,
            ErrorMessage = "Minimalna kwota zamówienia nie może być ujemna.")]
        public decimal? MinimalnaKwotaZamowienia { get; set; }

        [Range(0, int.MaxValue,
            ErrorMessage = "Maksymalna liczba użyć nie może być ujemna.")]
        public int? MaksLiczbaUzyc { get; set; }

        public bool CzyAktywny { get; set; }
    }
}