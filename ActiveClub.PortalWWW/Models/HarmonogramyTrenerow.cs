using System;
using System.Collections.Generic;

namespace ActiveClub.PortalWWW.Models;

public partial class HarmonogramyTrenerow
{
    public int HarmonogramId { get; set; }

    public int? TrenerId { get; set; }

    public DateTime? Data { get; set; }

    public virtual Trenerzy? Trener { get; set; }
}
