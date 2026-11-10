namespace ActiveClub.Shared.DTOs.RodzajeCzlonkostwa;

public class RodzajCzlonkostwaDto
{
    public int RodzajCzlonkostwaId { get; set; }
    public string Kod { get; set; } = "";
    public string Nazwa { get; set; } = "";
    public string? Opis { get; set; }
    public decimal? Cena { get; set; }
    public string TypRozliczenia { get; set; } = "";
    public int? OkresDni { get; set; }
    public int? LiczbaWejsc { get; set; }
    public bool CzyAktywny { get; set; }
    public bool CzyWidoczneOnline { get; set; }
    public DateTime DataUtworzenia { get; set; }
}