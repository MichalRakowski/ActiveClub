using System;
using System.Collections.Generic;

namespace ActiveClub.PortalWWW.Models;

public partial class RodzajeCzlonkostwa
{
    public int RodzajId { get; set; }

    public string? Nazwa { get; set; }

    public decimal? Cena { get; set; }

    public int? CzasTrwaniaDni { get; set; }

    public virtual ICollection<Czlonkostwa> Czlonkostwas { get; set; } = new List<Czlonkostwa>();
}
