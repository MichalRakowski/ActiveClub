namespace ActiveClub.Shared.DTOs.Zamowienia
{
    public class ZamowienieDto
    {
        public int ZamowienieId { get; set; }
        public string NumerZamowienia { get; set; } = string.Empty;
        public int UzytkownikId { get; set; }
        public int? KlubId { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal SumaBrutto { get; set; }
        public decimal KwotaRabatu { get; set; }
        public string Waluta { get; set; } = string.Empty;
        public int? MetodaPlatnosciId { get; set; }
        public int? RabatId { get; set; }
        public string TypKanalu { get; set; } = string.Empty;
        public DateTime DataUtworzenia { get; set; }
        public DateTime? DataOplacenia { get; set; }
        public int? UtworzylUzytkownikId { get; set; }
    }
}