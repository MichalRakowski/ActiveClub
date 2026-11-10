namespace ActiveClub.Shared.DTOs.Kluby;

public class KlubDto
{
    public int KlubId { get; set; }

    public string Nazwa { get; set; } = string.Empty;

    public string? Miasto { get; set; }

    public string? Ulica { get; set; }

    public string? KodPocztowy { get; set; }

    public string? Opis { get; set; }

    public string? Telefon { get; set; }

    public string? EmailKontakt { get; set; }   

    public bool CzyAktywny { get; set; }

    public DateTime DataUtworzenia { get; set; }   

    public DateTime? DataModyfikacji { get; set; } 
}