using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.MetodyPlatnosci
{
    public class UpdateMetodaPlatnosciDto
    {
        [Required(ErrorMessage = "Nazwa metody płatności jest wymagana.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Nazwa musi mieć od 2 do 100 znaków.")]
        public string Nazwa { get; set; } = string.Empty;

        public bool CzyAktywna { get; set; }
    }
}