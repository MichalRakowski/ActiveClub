using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.Raporty
{
    public class GenerateRaportDto
    {
        [Range(1, int.MaxValue,
            ErrorMessage = "Definicja raportu jest wymagana.")]
        public int RaportDefinicjaId { get; set; }

        [Required(ErrorMessage = "Format raportu jest wymagany.")]
        [RegularExpression("PDF|XLSX|CSV",
            ErrorMessage = "Nieprawidłowy format raportu.")]
        public string Format { get; set; } = "PDF";

        public DateTime? DataOd { get; set; }

        public DateTime? DataDo { get; set; }

        public int? KlubId { get; set; }

        public int? TrenerId { get; set; }

        public int? WygenerowalUzytkownikId { get; set; }
    }
}