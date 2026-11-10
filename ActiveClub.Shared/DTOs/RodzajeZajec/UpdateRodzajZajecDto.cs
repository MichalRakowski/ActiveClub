using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.RodzajeZajec
{
    public class UpdateRodzajZajecDto
    {
        [Required(ErrorMessage = "Nazwa zajęć jest wymagana.")]
        [StringLength(160, MinimumLength = 2,
            ErrorMessage = "Nazwa musi mieć od 2 do 160 znaków.")]
        public string Nazwa { get; set; } = string.Empty;

        [StringLength(500,
            ErrorMessage = "Opis może mieć maksymalnie 500 znaków.")]
        public string? Opis { get; set; }

        [Range(1, 1440,
            ErrorMessage = "Czas trwania musi być większy od 0.")]
        public int? CzasTrwaniaMin { get; set; }

        public bool CzyAktywne { get; set; }

        public bool CzyWidoczneOnline { get; set; }
    }
}