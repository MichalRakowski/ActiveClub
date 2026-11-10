using System;
using System.Collections.Generic;

namespace ActiveClub.PortalWWW.Models;

public partial class Platnosci
{
    public int PlatnoscId { get; set; }

    public int CzlonkostwoId { get; set; }

    public int MetodaId { get; set; }

    public decimal? Kwota { get; set; }

    public DateTime? DataPlatnosci { get; set; }

    public virtual Czlonkostwa Czlonkostwo { get; set; } = null!;

    public virtual MetodyPlatnosci Metoda { get; set; } = null!;
}
