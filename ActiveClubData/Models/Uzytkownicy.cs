using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class Uzytkownicy
{
    public int UzytkownikId { get; set; }

    public string? Imie { get; set; }

    public string? Nazwisko { get; set; }

    public string Email { get; set; } = null!;

    public DateTime DataUtworzenia { get; set; }

    public string? Telefon { get; set; }

    public virtual ICollection<CzlonkostwaUzytkownika> CzlonkostwaUzytkownikas { get; set; } = new List<CzlonkostwaUzytkownika>();

    public virtual ICollection<LogiLogowan> LogiLogowans { get; set; } = new List<LogiLogowan>();

    public virtual ICollection<Opinie> Opinies { get; set; } = new List<Opinie>();

    public virtual ICollection<RaportyWygenerowane> RaportyWygenerowanes { get; set; } = new List<RaportyWygenerowane>();

    public virtual ICollection<RejestracjeNaZajecium> RejestracjeNaZajecia { get; set; } = new List<RejestracjeNaZajecium>();

    public virtual ICollection<RezerwacjeTrenera> RezerwacjeTreneras { get; set; } = new List<RezerwacjeTrenera>();

    public virtual ICollection<Trenerzy> Trenerzies { get; set; } = new List<Trenerzy>();

    public virtual ICollection<UzycieRabatu> UzycieRabatus { get; set; } = new List<UzycieRabatu>();

    public virtual ICollection<UzytkownikRole> UzytkownikRoles { get; set; } = new List<UzytkownikRole>();

    public virtual ICollection<Zamowienium> Zamowienia { get; set; } = new List<Zamowienium>();
}
