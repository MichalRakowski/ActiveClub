namespace ActiveClub.Shared.DTOs.OfertyKlubuCzlonkostwa
{
    public class OfertaKlubuCzlonkostwoDto
    {
        public int OfertaKlubuCzlonkostwoId { get; set; }
        public int KlubId { get; set; }
        public string KlubNazwa { get; set; } = string.Empty;
        public int RodzajCzlonkostwaId { get; set; }
        public string RodzajCzlonkostwaNazwa { get; set; } = string.Empty;
        public decimal Cena { get; set; }
        public string Waluta { get; set; } = string.Empty;
        public bool CzyAktywna { get; set; }
        public bool CzyWidoczneOnline { get; set; }
        public DateTime? DataOd { get; set; }
        public DateTime? DataDo { get; set; }
        public int? Kolejnosc { get; set; }
        public string? OpisMarketingowy { get; set; }
        public DateTime DataUtworzenia { get; set; }
    }
}