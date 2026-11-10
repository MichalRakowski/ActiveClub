using System;
using System.Collections.Generic;

namespace ActiveClub.PortalWWW.Models;

public partial class HistoriaCzlonkostwa
{
    public int HistoriaId { get; set; }

    public int CzlonkostwoId { get; set; }

    public DateTime? DataZmiany { get; set; }

    public string? Opis { get; set; }

    public virtual Czlonkostwa Czlonkostwo { get; set; } = null!;
}
