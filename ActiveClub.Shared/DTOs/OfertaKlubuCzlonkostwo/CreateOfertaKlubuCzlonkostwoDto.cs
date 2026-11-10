using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.OfertyKlubuCzlonkostwa
{
    public class CreateOfertaKlubuCzlonkostwoDto
    {
        [Range(1, int.MaxValue,
            ErrorMessage = "Klub jest wymagany.")]
        public int KlubId { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Rodzaj członkostwa jest wymagany.")]
        public int RodzajCzlonkostwaId { get; set; }

        [Range(0.01, 999999,
            ErrorMessage = "Cena musi być większa od 0.")]
        public decimal Cena { get; set; }

        [Required(ErrorMessage = "Waluta jest wymagana.")]
        [StringLength(10,
            ErrorMessage = "Waluta może mieć maksymalnie 10 znaków.")]
        public string Waluta { get; set; } = string.Empty;

        public bool CzyAktywna { get; set; }

        public bool CzyWidoczneOnline { get; set; }

        public DateTime? DataOd { get; set; }

        public DateTime? DataDo { get; set; }

        [Range(0, int.MaxValue,
            ErrorMessage = "Kolejność nie może być ujemna.")]
        public int? Kolejnosc { get; set; }

        [StringLength(2000,
            ErrorMessage = "Opis marketingowy może mieć maksymalnie 2000 znaków.")]
        public string? OpisMarketingowy { get; set; }
    }
}