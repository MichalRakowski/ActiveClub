using System;
using System.Collections.Generic;

namespace ActiveClub.PortalWWW.Models;

public partial class HarmonogramZajec
{
    public int HarmonogramId { get; set; }

    public int? ZajeciaId { get; set; }

    public DateTime? Data { get; set; }

    public virtual ICollection<RejestracjeNaZajecium> RejestracjeNaZajecia { get; set; } = new List<RejestracjeNaZajecium>();

    public virtual Zajecium? Zajecia { get; set; }
}
