using System.ComponentModel.DataAnnotations;

namespace ActiveClub.PortalWWW.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Adres e-mail jest wymagany")]
        [EmailAddress(ErrorMessage = "Niepoprawny adres e-mail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}