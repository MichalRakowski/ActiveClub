using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveClub.Shared.DTOs.Sale
{
    public class SalaDto
    {
        public int SalaId { get; set; }
        public int KlubId { get; set; }
        public string KlubNazwa { get; set; } = string.Empty;
        public string Nazwa { get; set; } = string.Empty;
        public int? Pojemnosc { get; set; }
        public string? Opis { get; set; }
        public bool CzyAktywna { get; set; }
    }
}