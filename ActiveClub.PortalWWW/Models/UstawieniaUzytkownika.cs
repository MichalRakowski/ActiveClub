using System;
using System.Collections.Generic;

namespace ActiveClub.PortalWWW.Models;

public partial class UstawieniaUzytkownika
{
    public int UstawienieId { get; set; }

    public int UzytkownikId { get; set; }

    public string? Klucz { get; set; }

    public string? Wartosc { get; set; }

    public virtual Uzytkownicy Uzytkownik { get; set; } = null!;
}
