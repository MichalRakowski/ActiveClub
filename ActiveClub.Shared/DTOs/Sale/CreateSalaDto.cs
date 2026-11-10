using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.Sale
{
    public class CreateSalaDto
    {
        [Range(1, int.MaxValue,
            ErrorMessage = "Klub jest wymagany.")]
        public int KlubId { get; set; }

        [Required(ErrorMessage = "Nazwa sali jest wymagana.")]
        [StringLength(120, MinimumLength = 2,
            ErrorMessage = "Nazwa sali musi mieć od 2 do 120 znaków.")]
        public string Nazwa { get; set; } = string.Empty;

        [Range(1, 10000,
            ErrorMessage = "Pojemność musi być większa od 0.")]
        public int? Pojemnosc { get; set; }

        [StringLength(500,
            ErrorMessage = "Opis może mieć maksymalnie 500 znaków.")]
        public string? Opis { get; set; }

        public bool CzyAktywna { get; set; } = true;
    }
}