using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace ActiveClub.Shared.DTOs.Raporty
{
    public class RaportDefinicjaDto
    {
        public int RaportDefinicjaId { get; set; }
        public string KodRaportu { get; set; } = string.Empty;
        public string Nazwa { get; set; } = string.Empty;
        public string? Opis { get; set; }
        public string? ParametryJson { get; set; }
        public bool CzyAktywny { get; set; }
    }
}