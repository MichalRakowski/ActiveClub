using System;
using System.Collections.Generic;

namespace ActiveClub.PortalWWW.Models;

public partial class Czlonkostwa
{
    public int CzlonkostwoId { get; set; }

    public int UzytkownikId { get; set; }

    public int RodzajId { get; set; }

    public DateOnly? DataRozpoczecia { get; set; }

    public DateOnly? DataZakonczenia { get; set; }

    public virtual ICollection<HistoriaCzlonkostwa> HistoriaCzlonkostwas { get; set; } = new List<HistoriaCzlonkostwa>();

    public virtual ICollection<Platnosci> Platnoscis { get; set; } = new List<Platnosci>();

    public virtual RodzajeCzlonkostwa Rodzaj { get; set; } = null!;

    public virtual Uzytkownicy Uzytkownik { get; set; } = null!;
}
