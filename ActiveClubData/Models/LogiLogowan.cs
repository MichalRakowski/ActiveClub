using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class LogiLogowan
{
    public int LogowanieId { get; set; }

    public int? UzytkownikId { get; set; }

    public string? EmailWpisany { get; set; }

    public bool CzySukces { get; set; }

    public string? Ip { get; set; }

    public string? UserAgent { get; set; }

    public DateTime DataZdarzenia { get; set; }

    public virtual Uzytkownicy? Uzytkownik { get; set; }
}
