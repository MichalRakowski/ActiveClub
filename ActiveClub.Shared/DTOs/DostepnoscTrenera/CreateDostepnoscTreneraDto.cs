using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.DostepnoscTrenera
{
    public class CreateDostepnoscTreneraDto
    {
        [Range(1, int.MaxValue,
            ErrorMessage = "Trener jest wymagany.")]
        public int TrenerId { get; set; }

        public int? KlubId { get; set; }

        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        [Required(ErrorMessage = "Status jest wymagany.")]
        [RegularExpression(
            "DOSTEPNY|NIEDOSTEPNY|URLOP",
            ErrorMessage = "Nieprawidłowy status.")]
        public string? Status { get; set; }
    }
}