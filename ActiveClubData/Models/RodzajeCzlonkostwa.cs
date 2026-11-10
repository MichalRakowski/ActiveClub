using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class RodzajeCzlonkostwa
{
    public int RodzajCzlonkostwaId { get; set; }

    public string Kod { get; set; } = null!;

    public string Nazwa { get; set; } = null!;

    public string? Opis { get; set; }
    public decimal? Cena { get; set; }
    public string TypRozliczenia { get; set; } = null!;

    public int? OkresDni { get; set; }

    public int? LiczbaWejsc { get; set; }

    public bool CzyAktywny { get; set; }

    public bool CzyWidoczneOnline { get; set; }

    public DateTime DataUtworzenia { get; set; }
    public virtual ICollection<CzlonkostwaUzytkownika> CzlonkostwaUzytkownikas { get; set; } = new List<CzlonkostwaUzytkownika>();

    public virtual ICollection<OfertaKlubuCzlonkostwo> OfertaKlubuCzlonkostwos { get; set; } = new List<OfertaKlubuCzlonkostwo>();
}
