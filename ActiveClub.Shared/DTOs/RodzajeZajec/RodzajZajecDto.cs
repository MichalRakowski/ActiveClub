using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveClub.Shared.DTOs.RodzajeZajec
{
    public class RodzajZajecDto
    {
        public int RodzajZajecId { get; set; }
        public string Nazwa { get; set; } = string.Empty;
        public string? Opis { get; set; }
        public int? CzasTrwaniaMin { get; set; }
        public bool CzyAktywne { get; set; }
        public bool CzyWidoczneOnline { get; set; }
    }
}