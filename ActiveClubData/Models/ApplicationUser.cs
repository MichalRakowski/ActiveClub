using Microsoft.AspNetCore.Identity;
using System;

namespace ActiveClubData.Models;

public class ApplicationUser : IdentityUser<int>
{
    public string Imie { get; set; } = null!;
    public string Nazwisko { get; set; } = null!;
    public bool CzyAktywny { get; set; } = true;
    public int? KlubDomyslnyId { get; set; }
    public string? Uwagi { get; set; }

    public virtual ICollection<Aktualnosci> Aktualnoscis { get; set; } = new List<Aktualnosci>();
    public virtual ICollection<CzlonkostwaUzytkownika> CzlonkostwaUzytkownikas { get; set; } = new List<CzlonkostwaUzytkownika>();
    public virtual ICollection<Opinie> Opinies { get; set; } = new List<Opinie>();
    public virtual ICollection<PracownicyKlubu> PracownicyKlubus { get; set; } = new List<PracownicyKlubu>();
    public virtual ICollection<RaportyWygenerowane> RaportyWygenerowanes { get; set; } = new List<RaportyWygenerowane>();
    public virtual ICollection<RejestracjeNaZajecium> RejestracjeNaZajecia { get; set; } = new List<RejestracjeNaZajecium>();
    public virtual ICollection<RezerwacjeTrenera> RezerwacjeTreneras { get; set; } = new List<RezerwacjeTrenera>();
    public virtual ICollection<Trenerzy> Trenerzies { get; set; } = new List<Trenerzy>();
    public virtual ICollection<Zamowienium> Zamowienia { get; set; } = new List<Zamowienium>();
}