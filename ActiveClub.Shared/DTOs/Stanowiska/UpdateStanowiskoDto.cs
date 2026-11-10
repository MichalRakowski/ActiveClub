using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.Stanowiska
{
    public class UpdateStanowiskoDto
    {
        [Required(ErrorMessage = "Nazwa stanowiska jest wymagana.")]
        [StringLength(80, MinimumLength = 2,
            ErrorMessage = "Nazwa musi mieć od 2 do 80 znaków.")]
        public string Nazwa { get; set; } = string.Empty;

        [StringLength(255,
            ErrorMessage = "Opis może mieć maksymalnie 255 znaków.")]
        public string? Opis { get; set; }

        public bool CzyAktywne { get; set; }
    }
}