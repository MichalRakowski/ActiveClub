using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.Aktualnosci
{
    public class CreateAktualnoscDto
    {
        [Required(ErrorMessage = "Tytuł jest wymagany.")]
        [StringLength(200, MinimumLength = 3,
            ErrorMessage = "Tytuł musi mieć od 3 do 200 znaków.")]
        public string Tytul { get; set; } = null!;

        [StringLength(10000,
            ErrorMessage = "Treść może mieć maksymalnie 10000 znaków.")]
        public string? Tresc { get; set; }

        public int? KlubId { get; set; }

        public bool? CzyWidoczna { get; set; }

        public DateTime? DataPublikacji { get; set; }

        public DateTime DataDodania { get; set; }

        public int? DodanePrzezUzytkownikId { get; set; }
    }
}