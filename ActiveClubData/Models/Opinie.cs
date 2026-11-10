using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class Opinie
{
    public int OpiniaId { get; set; }

    public int KlubId { get; set; }

    public int UzytkownikId { get; set; }

    public byte Ocena { get; set; }

    public string? Komentarz { get; set; }

    public bool CzyWidoczna { get; set; }

    public DateTime DataDodania { get; set; }

    public virtual Kluby Klub { get; set; } = null!;

    public virtual ApplicationUser Uzytkownik { get; set; } = null!;
}
