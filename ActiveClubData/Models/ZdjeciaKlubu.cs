using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class ZdjeciaKlubu
{
    public int ZdjecieId { get; set; }

    public int KlubId { get; set; }

    public string Url { get; set; } = null!;

    public int Kolejnosc { get; set; }

    public string? Opis { get; set; }

    public DateTime DataDodania { get; set; }

    public virtual Kluby Klub { get; set; } = null!;
}
