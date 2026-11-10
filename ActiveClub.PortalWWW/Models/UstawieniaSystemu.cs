using System;
using System.Collections.Generic;

namespace ActiveClub.PortalWWW.Models;

public partial class UstawieniaSystemu
{
    public int UstawienieId { get; set; }

    public string? Klucz { get; set; }

    public string? Wartosc { get; set; }
}
