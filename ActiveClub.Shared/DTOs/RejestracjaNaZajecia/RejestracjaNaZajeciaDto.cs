using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveClub.Shared.DTOs.RejestracjeNaZajecia
{
    public class RejestracjaNaZajeciaDto
    {
        public int RejestracjaId { get; set; }
        public int HarmonogramId { get; set; }
        public int UzytkownikId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? DataRejestracji { get; set; }
        public string Zrodlo { get; set; } = string.Empty;

        // opcjonalnie do wyświetlania na liście:
        public string HarmonogramNazwa { get; set; } = string.Empty;
        public string UzytkownikNazwa { get; set; } = string.Empty;
    }
}