using System;
using System.Collections.Generic;

namespace ActiveClub.PortalWWW.Models;

public partial class Rabaty
{
    public int RabatId { get; set; }

    public string? Nazwa { get; set; }

    public int? Procent { get; set; }
}
