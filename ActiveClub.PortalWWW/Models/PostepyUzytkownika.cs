using System;
using System.Collections.Generic;

namespace ActiveClub.PortalWWW.Models;

public partial class PostepyUzytkownika
{
    public int PostepId { get; set; }

    public int? UzytkownikId { get; set; }

    public string? Opis { get; set; }

    public DateOnly? Data { get; set; }

    public virtual Uzytkownicy? Uzytkownik { get; set; }
}
