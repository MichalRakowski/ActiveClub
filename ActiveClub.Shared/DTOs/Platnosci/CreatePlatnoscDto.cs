using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.Platnosci
{
    public class CreatePlatnoscDto
    {
        [Range(1, int.MaxValue,
            ErrorMessage = "Nieprawidłowe zamówienie.")]
        public int ZamowienieId { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Nieprawidłowa metoda płatności.")]
        public int MetodaPlatnosciId { get; set; }

        [Range(0.01, 999999,
            ErrorMessage = "Kwota musi być większa od 0.")]
        public decimal Kwota { get; set; }

        [Required(ErrorMessage = "Waluta jest wymagana.")]
        [StringLength(10,
            ErrorMessage = "Waluta może mieć maksymalnie 10 znaków.")]
        public string Waluta { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status płatności jest wymagany.")]
        [RegularExpression(
            "Nowa|Oczekująca|Opłacona|Anulowana|Odrzucona|Zwrot",
            ErrorMessage = "Nieprawidłowy status płatności.")]
        public string Status { get; set; } = string.Empty;

        [Required(ErrorMessage = "Provider płatności jest wymagany.")]
        [StringLength(100,
            ErrorMessage = "Provider może mieć maksymalnie 100 znaków.")]
        public string Provider { get; set; } = string.Empty;

        [StringLength(200,
            ErrorMessage = "Id transakcji może mieć maksymalnie 200 znaków.")]
        public string ProviderTransactionId { get; set; } = string.Empty;

        public DateTime? DataPlatnosci { get; set; }

        public DateTime? DataUtworzenia { get; set; }

        [StringLength(1000,
            ErrorMessage = "Uwagi mogą mieć maksymalnie 1000 znaków.")]
        public string Uwagi { get; set; } = string.Empty;
    }
}