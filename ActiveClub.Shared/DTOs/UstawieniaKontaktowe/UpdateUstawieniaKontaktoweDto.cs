using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.UstawieniaKontaktowe
{
    public class UpdateUstawieniaKontaktoweDto
    {
        [Required(ErrorMessage = "Email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Podaj poprawny email.")]
        [StringLength(320)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon jest wymagany.")]
        [StringLength(30)]
        public string Telefon { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adres jest wymagany.")]
        [StringLength(500)]
        public string Adres { get; set; } = string.Empty;

        [Required(ErrorMessage = "Godziny kontaktu są wymagane.")]
        [StringLength(200)]
        public string GodzinyKontaktu { get; set; } = string.Empty;
    }
}