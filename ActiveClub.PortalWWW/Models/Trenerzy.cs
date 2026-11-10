using System;
using System.Collections.Generic;

namespace ActiveClub.PortalWWW.Models;

public partial class Trenerzy
{
    public int TrenerId { get; set; }

    public string? Imie { get; set; }

    public string? Nazwisko { get; set; }

    public virtual ICollection<HarmonogramyTrenerow> HarmonogramyTrenerows { get; set; } = new List<HarmonogramyTrenerow>();

    public virtual ICollection<Kluby> Klubs { get; set; } = new List<Kluby>();
}
