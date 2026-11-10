using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class Rabaty
{
    public int RabatId { get; set; }

    public string Kod { get; set; } = null!;

    public string Typ { get; set; } = null!;

    public decimal Wartosc { get; set; }

    public DateOnly? DataOd { get; set; }

    public DateOnly? DataDo { get; set; }

    public decimal? MinimalnaKwotaZamowienia { get; set; }

    public int? MaksLiczbaUzyc { get; set; }

    public bool CzyAktywny { get; set; }

    public virtual ICollection<Zamowienium> Zamowienia { get; set; } = new List<Zamowienium>();
}
