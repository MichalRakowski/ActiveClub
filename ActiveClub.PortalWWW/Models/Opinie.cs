using System;
using System.Collections.Generic;

namespace ActiveClub.PortalWWW.Models;

public partial class Opinie
{
    public int OpiniaId { get; set; }

    public int? UzytkownikId { get; set; }

    public string? Tresc { get; set; }

    public virtual Uzytkownicy? Uzytkownik { get; set; }
}
