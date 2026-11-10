using System;
using System.Collections.Generic;

namespace ActiveClub.PortalWWW.Models;

public partial class Kluby
{
    public int KlubId { get; set; }

    public string? Nazwa { get; set; }

    public string? Adres { get; set; }

    public virtual ICollection<Trenerzy> Treners { get; set; } = new List<Trenerzy>();
}
