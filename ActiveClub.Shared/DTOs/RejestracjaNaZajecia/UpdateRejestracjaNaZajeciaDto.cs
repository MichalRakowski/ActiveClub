using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.RejestracjeNaZajecia
{
    public class UpdateRejestracjaNaZajeciaDto
    {
        [Range(1, int.MaxValue,
            ErrorMessage = "Harmonogram jest wymagany.")]
        public int HarmonogramId { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Użytkownik jest wymagany.")]
        public int UzytkownikId { get; set; }

        [Required(ErrorMessage = "Status jest wymagany.")]
        [RegularExpression(
    "NOWA|POTWIERDZONA|ANULOWANA|OBECNY|NIEOBECNY",
    ErrorMessage = "Nieprawidłowy status.")]
        public string? Status { get; set; }

        public DateTime? DataRejestracji { get; set; }

        [StringLength(50,
            ErrorMessage = "Źródło może mieć maksymalnie 50 znaków.")]
        public string? Zrodlo { get; set; }
    }
}