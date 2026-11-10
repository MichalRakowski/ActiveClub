using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class TrenerKlub
{
    public int TrenerId { get; set; }

    public int KlubId { get; set; }

    public DateOnly? DataOd { get; set; }

    public DateOnly? DataDo { get; set; }

    public virtual Kluby Klub { get; set; } = null!;

    public virtual Trenerzy Trener { get; set; } = null!;
}
