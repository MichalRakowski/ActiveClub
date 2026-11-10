namespace ActiveClub.Shared.DTOs.Harmonogram;

public class HarmonogramZajecListDto
{
    public int HarmonogramId { get; set; }

    public int KlubId { get; set; }
    public string KlubNazwa { get; set; } = string.Empty;

    public int RodzajZajecId { get; set; }
    public string RodzajZajecNazwa { get; set; } = string.Empty;
    public string? RodzajZajecOpis { get; set; }

    public int? SalaId { get; set; }

    public int? TrenerId { get; set; }
    public string? TrenerImie { get; set; }
    public string? TrenerNazwisko { get; set; }

    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }

    public int? LimitMiejsc { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool CzyWidoczneOnline { get; set; }
    public int LiczbaZapisanych { get; set; }
    public int LiczbaWolnychMiejsc { get; set; }
}