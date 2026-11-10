using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class MetodyPlatnosci
{
    public int MetodaPlatnosciId { get; set; }

    public string Nazwa { get; set; } = null!;

    public bool CzyAktywna { get; set; }

    public virtual ICollection<Platnosci> Platnoscis { get; set; } = new List<Platnosci>();

    public virtual ICollection<Zamowienium> Zamowienia { get; set; } = new List<Zamowienium>();
}
