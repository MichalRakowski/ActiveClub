using System;
using System.Collections.Generic;

namespace ActiveClub.PortalWWW.Models;

public partial class StatusSprzetu
{
    public int StatusId { get; set; }

    public int? SprzetId { get; set; }

    public string? Status { get; set; }

    public virtual Sprzet? Sprzet { get; set; }
}
