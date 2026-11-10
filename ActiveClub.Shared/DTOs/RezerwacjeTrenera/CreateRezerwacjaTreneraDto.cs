using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.RezerwacjeTrenera
{
    public class CreateRezerwacjaTreneraDto
    {
        [Range(1, int.MaxValue,
            ErrorMessage = "Trener jest wymagany.")]
        public int TrenerId { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Użytkownik jest wymagany.")]
        public int UzytkownikId { get; set; }

        public int? KlubId { get; set; }

        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        [Required(ErrorMessage = "Status jest wymagany.")]
        [RegularExpression(
            "NOWA|POTWIERDZONA|ANULOWANA|ZAKONCZONA",
            ErrorMessage = "Nieprawidłowy status.")]
        public string? Status { get; set; }

        [Range(0, 99999,
            ErrorMessage = "Cena nie może być ujemna.")]
        public decimal? Cena { get; set; }
    }
}