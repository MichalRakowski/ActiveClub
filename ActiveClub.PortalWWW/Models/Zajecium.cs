using System;
using System.Collections.Generic;

namespace ActiveClub.PortalWWW.Models;

public partial class Zajecium
{
    public int ZajeciaId { get; set; }

    public string? Nazwa { get; set; }

    public string? Opis { get; set; }

    public virtual ICollection<HarmonogramZajec> HarmonogramZajecs { get; set; } = new List<HarmonogramZajec>();
}
