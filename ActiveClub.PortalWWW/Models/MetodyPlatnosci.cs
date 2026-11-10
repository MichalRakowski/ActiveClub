using System;
using System.Collections.Generic;

namespace ActiveClub.PortalWWW.Models;

public partial class MetodyPlatnosci
{
    public int MetodaId { get; set; }

    public string? Nazwa { get; set; }

    public virtual ICollection<Platnosci> Platnoscis { get; set; } = new List<Platnosci>();
}
