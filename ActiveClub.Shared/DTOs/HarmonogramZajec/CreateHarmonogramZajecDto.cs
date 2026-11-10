using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.HarmonogramZajec
{
    public class CreateHarmonogramZajecDto
    {
        [Range(1, int.MaxValue,
            ErrorMessage = "Klub jest wymagany.")]
        public int KlubId { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Rodzaj zajęć jest wymagany.")]
        public int RodzajZajecId { get; set; }

        public int? SalaId { get; set; }

        public int? TrenerId { get; set; }

        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        [Range(1, 1000,
            ErrorMessage = "Limit miejsc musi być większy od 0.")]
        public int? LimitMiejsc { get; set; }

        [Required(ErrorMessage = "Status jest wymagany.")]
        [RegularExpression(
            "AKTYWNE|ANULOWANE|ZAKONCZONE",
            ErrorMessage = "Nieprawidłowy status.")]
        public string Status { get; set; } = string.Empty;

        public bool CzyWidoczneOnline { get; set; }

        public DateTime DataUtworzenia { get; set; }

        public int? UtworzylUzytkownikId { get; set; }
    }
}