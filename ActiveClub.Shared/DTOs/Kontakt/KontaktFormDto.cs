using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.Kontakt;

public class KontaktFormDto
{
    [Required]
    [StringLength(100)]
    public string ImieNazwisko { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    public string Temat { get; set; } = string.Empty;

    [Required]
    [StringLength(3000)]
    public string Wiadomosc { get; set; } = string.Empty;
}