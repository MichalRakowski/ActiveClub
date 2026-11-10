using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class GodzinyOtwarcium
{
    public int GodzinyOtwarciaId { get; set; }

    public int KlubId { get; set; }

    public byte DzienTygodnia { get; set; }

    public TimeOnly? GodzinaOd { get; set; }

    public TimeOnly? GodzinaDo { get; set; }

    public bool CzyZamkniete { get; set; }

    public virtual Kluby Klub { get; set; } = null!;
}
