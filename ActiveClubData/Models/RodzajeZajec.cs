using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class RodzajeZajec
{
    public int RodzajZajecId { get; set; }

    public string Nazwa { get; set; } = null!;

    public string? Opis { get; set; }

    public int? CzasTrwaniaMin { get; set; }

    public bool CzyAktywne { get; set; }

    public bool CzyWidoczneOnline { get; set; }

    public virtual ICollection<HarmonogramZajec> HarmonogramZajecs { get; set; } = new List<HarmonogramZajec>();
}
