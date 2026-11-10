using System;
using System.Collections.Generic;

namespace ActiveClub.PortalWWW.Models;

public partial class RejestracjeNaZajecium
{
    public int RejestracjaId { get; set; }

    public int? HarmonogramId { get; set; }

    public int? UzytkownikId { get; set; }

    public virtual HarmonogramZajec? Harmonogram { get; set; }

    public virtual Uzytkownicy? Uzytkownik { get; set; }
}
