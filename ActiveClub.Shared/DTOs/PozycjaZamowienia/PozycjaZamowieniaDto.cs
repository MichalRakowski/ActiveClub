namespace ActiveClub.Shared.DTOs.PozycjeZamowienia
{
    public class PozycjaZamowieniaDto
    {
        public int PozycjaId { get; set; }
        public int ZamowienieId { get; set; }
        public string NumerZamowienia { get; set; } = string.Empty;
        public string Typ { get; set; } = string.Empty;
        public int? RefId { get; set; }
        public string Nazwa { get; set; } = string.Empty;
        public string? Opis { get; set; }
        public int Ilosc { get; set; }
        public decimal CenaJedn { get; set; }
        public decimal KwotaBrutto { get; set; }
    }
}