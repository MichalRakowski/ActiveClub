namespace ActiveClub.Shared.DTOs.Uzytkownicy
{
    public class UzytkownikDto
    {
        public int Id { get; set; }

        public string Imie { get; set; } = string.Empty;
        public string Nazwisko { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefon { get; set; }

        public bool CzyAktywny { get; set; }

        public int? KlubDomyslnyId { get; set; }
        public string? Uwagi { get; set; }

        public string? Rola { get; set; }
        public int? KlubId { get; set; }
        public int? StanowiskoId { get; set; }

        public string ImieNazwisko => $"{Imie} {Nazwisko}".Trim();

        public string DisplayName =>
            string.IsNullOrWhiteSpace(Email)
                ? ImieNazwisko
                : $"{ImieNazwisko} ({Email})";
    }
}