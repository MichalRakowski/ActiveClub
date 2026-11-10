namespace ActiveClub.Shared.DTOs.CzlonkostwaUzytkownika
{
    public class CzlonkostwoUzytkownikaDto
    {
        public int CzlonkostwoId { get; set; }
        public int UzytkownikId { get; set; }
        public string UzytkownikNazwa { get; set; } = string.Empty;

        public int? OfertaKlubuCzlonkostwoId { get; set; }
        public string OfertaKlubuCzlonkostwoNazwa { get; set; } = string.Empty;

        public int? RodzajCzlonkostwaId { get; set; }
        public string RodzajCzlonkostwaNazwa { get; set; } = string.Empty;

        public int? KlubId { get; set; }
        public string KlubNazwa { get; set; } = string.Empty;

        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }

        public string? Status { get; set; }
        public decimal? CenaZakupu { get; set; }

        public int? ZamowienieId { get; set; }
        public string ZamowienieNazwa { get; set; } = string.Empty;

        public int? NadanePrzezUzytkownikId { get; set; }
        public string NadanePrzezUzytkownikNazwa { get; set; } = string.Empty;

        public DateTime? DataUtworzenia { get; set; }
        public DateTime? DataAktywacji { get; set; }
        public DateTime? DataAnulowania { get; set; }
        public string? PowodAnulowania { get; set; }
        public int? LiczbaWejscLimit { get; set; }

        public int LiczbaWejscWykorzystanych { get; set; }

        public int PozostaloWejsc =>
            (LiczbaWejscLimit ?? 0) - LiczbaWejscWykorzystanych;
    }
}