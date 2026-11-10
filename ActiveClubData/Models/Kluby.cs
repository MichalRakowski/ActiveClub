using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class Kluby
{
    public int KlubId { get; set; }

    public string Nazwa { get; set; } = null!;

    public string? Miasto { get; set; }

    public string? Ulica { get; set; }

    public string? KodPocztowy { get; set; }

    public string? Telefon { get; set; }

    public string? EmailKontakt { get; set; }

    public string? Opis { get; set; }

    public bool CzyAktywny { get; set; }

    public DateTime DataUtworzenia { get; set; }

    public DateTime? DataModyfikacji { get; set; }

    public virtual ICollection<Aktualnosci> Aktualnoscis { get; set; } = new List<Aktualnosci>();

    public virtual ICollection<CzlonkostwaUzytkownika> CzlonkostwaUzytkownikas { get; set; } = new List<CzlonkostwaUzytkownika>();

    public virtual ICollection<DostepnoscTrenera> DostepnoscTreneras { get; set; } = new List<DostepnoscTrenera>();

    public virtual ICollection<GodzinyOtwarcium> GodzinyOtwarcia { get; set; } = new List<GodzinyOtwarcium>();

    public virtual ICollection<HarmonogramZajec> HarmonogramZajecs { get; set; } = new List<HarmonogramZajec>();

    public virtual ICollection<OfertaKlubuCzlonkostwo> OfertaKlubuCzlonkostwos { get; set; } = new List<OfertaKlubuCzlonkostwo>();

    public virtual ICollection<Opinie> Opinies { get; set; } = new List<Opinie>();

    public virtual ICollection<PracownicyKlubu> PracownicyKlubus { get; set; } = new List<PracownicyKlubu>();

    public virtual ICollection<RezerwacjeTrenera> RezerwacjeTreneras { get; set; } = new List<RezerwacjeTrenera>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual ICollection<TrenerKlub> TrenerKlubs { get; set; } = new List<TrenerKlub>();

    public virtual ICollection<Zamowienium> Zamowienia { get; set; } = new List<Zamowienium>();

   
}
