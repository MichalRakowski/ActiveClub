using System;
using System.Collections.Generic;
using System.Linq;
namespace ActiveClub.Shared.DTOs.Stanowiska
{
    public class StanowiskoDto
    {
        public int StanowiskoId { get; set; }
        public string Nazwa { get; set; } = string.Empty;
        public string? Opis { get; set; }
        public bool CzyAktywne { get; set; }
    }
}