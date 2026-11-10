using System;
using System.Collections.Generic;

namespace ActiveClub.PortalWWW.Models;

public partial class Uzytkownicy
{
    public int UzytkownikId { get; set; }

    public string Imie { get; set; } = null!;

    public string Nazwisko { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime? DataUtworzenia { get; set; }

    public virtual ICollection<Czlonkostwa> Czlonkostwas { get; set; } = new List<Czlonkostwa>();

    public virtual ICollection<Opinie> Opinies { get; set; } = new List<Opinie>();

    public virtual ICollection<PostepyUzytkownika> PostepyUzytkownikas { get; set; } = new List<PostepyUzytkownika>();

    public virtual ICollection<RejestracjeNaZajecium> RejestracjeNaZajecia { get; set; } = new List<RejestracjeNaZajecium>();

    public virtual ICollection<UstawieniaUzytkownika> UstawieniaUzytkownikas { get; set; } = new List<UstawieniaUzytkownika>();

    public virtual ICollection<Role> Rolas { get; set; } = new List<Role>();
}
