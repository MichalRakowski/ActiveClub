using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class DostepnoscTrenera
{
    public int DostepnoscId { get; set; }

    public int TrenerId { get; set; }

    public int? KlubId { get; set; }

    public DateTime StartAt { get; set; }

    public DateTime EndAt { get; set; }

    public string Status { get; set; } = null!;

    public DateTime UtworzonoAt { get; set; }

    public virtual Kluby? Klub { get; set; }

    public virtual Trenerzy Trener { get; set; } = null!;
}
