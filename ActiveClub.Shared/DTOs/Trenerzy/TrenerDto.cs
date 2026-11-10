namespace ActiveClub.Shared.DTOs.Trenerzy
{
    public class TrenerDto
    {
        public int TrenerId { get; set; }
        public int? UzytkownikId { get; set; }

        public string Imie { get; set; } = string.Empty;
        public string Nazwisko { get; set; } = string.Empty;

        public string? Email { get; set; }
        public string? Telefon { get; set; }

        public string? Bio { get; set; }
        public string? SpecjalizacjaOpis { get; set; }
        public decimal? StawkaZaGodzine { get; set; }

        public bool CzyAktywny { get; set; }
        public DateTime DataUtworzenia { get; set; }

        public string ImieNazwisko => $"{Imie} {Nazwisko}".Trim();
        public List<int> KlubIds { get; set; } = new();

        public string KlubyNazwy { get; set; } = string.Empty;
    }
}