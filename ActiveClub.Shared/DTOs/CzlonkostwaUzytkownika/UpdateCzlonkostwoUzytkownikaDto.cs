using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.CzlonkostwaUzytkownika
{
    public class UpdateCzlonkostwoUzytkownikaDto
    {
        [Range(1, int.MaxValue,
            ErrorMessage = "Użytkownik jest wymagany.")]
        public int UzytkownikId { get; set; }

        public int? OfertaKlubuCzlonkostwoId { get; set; }

        public int? RodzajCzlonkostwaId { get; set; }

        public int? KlubId { get; set; }

        public DateTime? StartAt { get; set; }

        public DateTime? EndAt { get; set; }

        [Required(ErrorMessage = "Status jest wymagany.")]
        [RegularExpression(
            "NOWE|AKTYWNE|WYGASLE|ANULOWANE",
            ErrorMessage = "Nieprawidłowy status.")]
        public string? Status { get; set; }

        [Range(0, 999999,
            ErrorMessage = "Cena zakupu nie może być ujemna.")]
        public decimal? CenaZakupu { get; set; }

        public int? ZamowienieId { get; set; }

        public int? NadanePrzezUzytkownikId { get; set; }

        public DateTime? DataUtworzenia { get; set; }

        public DateTime? DataAktywacji { get; set; }

        public DateTime? DataAnulowania { get; set; }

        [StringLength(1000,
            ErrorMessage = "Powód anulowania może mieć maksymalnie 1000 znaków.")]
        public string? PowodAnulowania { get; set; }
    }
}