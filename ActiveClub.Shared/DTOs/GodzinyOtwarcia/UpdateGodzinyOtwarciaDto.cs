using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.GodzinyOtwarcia
{
    public class UpdateGodzinyOtwarciaDto
    {
        [Range(1, int.MaxValue,
            ErrorMessage = "Klub jest wymagany.")]
        public int KlubId { get; set; }

        [Range(0, 6,
            ErrorMessage = "Dzień tygodnia musi być w zakresie 0-6.")]
        public byte DzienTygodnia { get; set; }

        [StringLength(5,
            ErrorMessage = "Godzina od musi mieć format HH:mm.")]
        public string? GodzinaOd { get; set; }

        [StringLength(5,
            ErrorMessage = "Godzina do musi mieć format HH:mm.")]
        public string? GodzinaDo { get; set; }

        public bool CzyZamkniete { get; set; }
    }
}