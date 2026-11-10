using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class RaportyDefinicje
{
    public int RaportDefinicjaId { get; set; }

    public string KodRaportu { get; set; } = null!;

    public string Nazwa { get; set; } = null!;

    public string? Opis { get; set; }

    public string? ParametryJson { get; set; }

    public bool CzyAktywny { get; set; }
    public virtual ICollection<RaportyWygenerowane> RaportyWygenerowanes { get; set; } = new List<RaportyWygenerowane>();
}
