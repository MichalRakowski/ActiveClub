using System;
using System.Collections.Generic;
namespace ActiveClub.Shared.DTOs.Raporty
{
    public class RaportWygenerowanyDto
    {
        public int RaportWygenerowanyId { get; set; }
        public int RaportDefinicjaId { get; set; }
        public string NazwaRaportu { get; set; } = string.Empty;
        public int? WygenerowalUzytkownikId { get; set; }
        public string Format { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? PlikUrl { get; set; }
        public string? ParametryJson { get; set; }
        public DateTime? DataGeneracji { get; set; }
    }
}