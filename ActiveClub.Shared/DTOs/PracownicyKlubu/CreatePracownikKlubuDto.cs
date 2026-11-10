using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.PracownicyKlubu
{
    public class CreatePracownikKlubuDto
    {
        [Range(1, int.MaxValue,
            ErrorMessage = "Użytkownik jest wymagany.")]
        public int UzytkownikId { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Klub jest wymagany.")]
        public int KlubId { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Stanowisko jest wymagane.")]
        public int StanowiskoId { get; set; }

        public DateTime? DataOd { get; set; }

        public DateTime? DataDo { get; set; }

        public bool CzyAktywny { get; set; }

        [StringLength(1000,
            ErrorMessage = "Uwagi mogą mieć maksymalnie 1000 znaków.")]
        public string? Uwagi { get; set; }
    }
}