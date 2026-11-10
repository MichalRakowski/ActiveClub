using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class OfertaKlubuCzlonkostwo
{
    public int OfertaKlubuCzlonkostwoId { get; set; }

    public int KlubId { get; set; }

    public int RodzajCzlonkostwaId { get; set; }

    public decimal Cena { get; set; }

    public string Waluta { get; set; } = null!;

    public bool CzyAktywna { get; set; }

    public bool CzyWidoczneOnline { get; set; }

    public DateOnly? DataOd { get; set; }

    public DateOnly? DataDo { get; set; }

    public int? Kolejnosc { get; set; }

    public string? OpisMarketingowy { get; set; }

    public DateTime DataUtworzenia { get; set; }

    public virtual ICollection<CzlonkostwaUzytkownika> CzlonkostwaUzytkownikas { get; set; } = new List<CzlonkostwaUzytkownika>();

    public virtual Kluby Klub { get; set; } = null!;

    public virtual RodzajeCzlonkostwa RodzajCzlonkostwa { get; set; } = null!;
}
