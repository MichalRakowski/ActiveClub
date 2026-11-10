using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class Sale
{
    public int SalaId { get; set; }

    public int KlubId { get; set; }

    public string Nazwa { get; set; } = null!;

    public int? Pojemnosc { get; set; }

    public string? Opis { get; set; }

    public bool CzyAktywna { get; set; }

    public virtual ICollection<HarmonogramZajec> HarmonogramZajecs { get; set; } = new List<HarmonogramZajec>();

    public virtual Kluby Klub { get; set; } = null!;
}
