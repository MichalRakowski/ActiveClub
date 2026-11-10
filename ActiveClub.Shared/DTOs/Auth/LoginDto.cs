using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Login jest wymagany")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hasło jest wymagane")]
        public string Haslo { get; set; } = string.Empty;
    }
}