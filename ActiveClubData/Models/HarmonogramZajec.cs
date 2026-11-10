using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class HarmonogramZajec
{
    public int HarmonogramId { get; set; }

    public int KlubId { get; set; }

    public int RodzajZajecId { get; set; }

    public int? SalaId { get; set; }

    public int? TrenerId { get; set; }

    public DateTime StartAt { get; set; }

    public DateTime EndAt { get; set; }

    public int? LimitMiejsc { get; set; }

    public string Status { get; set; } = null!;

    public bool CzyWidoczneOnline { get; set; }

    public DateTime DataUtworzenia { get; set; }

    public int? UtworzylUzytkownikId { get; set; }

    public virtual Kluby Klub { get; set; } = null!;

    public virtual ICollection<RejestracjeNaZajecium> RejestracjeNaZajecia { get; set; } = new List<RejestracjeNaZajecium>();

    public virtual RodzajeZajec RodzajZajec { get; set; } = null!;

    public virtual Sale? Sala { get; set; }

    public virtual Trenerzy? Trener { get; set; }
}
