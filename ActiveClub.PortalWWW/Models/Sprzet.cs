using System;
using System.Collections.Generic;

namespace ActiveClub.PortalWWW.Models;

public partial class Sprzet
{
    public int SprzetId { get; set; }

    public string? Nazwa { get; set; }

    public virtual ICollection<StatusSprzetu> StatusSprzetus { get; set; } = new List<StatusSprzetu>();
}
