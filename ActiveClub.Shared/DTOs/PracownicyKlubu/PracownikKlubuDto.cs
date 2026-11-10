using System;
using System.Collections.Generic;
namespace ActiveClub.Shared.DTOs.PracownicyKlubu
{
    public class PracownikKlubuDto
    {
        public int PracownikKlubId { get; set; }
        public int UzytkownikId { get; set; }
        public int KlubId { get; set; }
        public int StanowiskoId { get; set; }

        public string UzytkownikNazwa { get; set; } = string.Empty;
        public string KlubNazwa { get; set; } = string.Empty;
        public string StanowiskoNazwa { get; set; } = string.Empty;

        public DateTime? DataOd { get; set; }
        public DateTime? DataDo { get; set; }
        public bool CzyAktywny { get; set; }
        public string? Uwagi { get; set; }
    }
}