using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class UzytkownikRole
{
    public int UzytkownikId { get; set; }

    public int RolaId { get; set; }

    public DateTime DataNadania { get; set; }

    public virtual Role Rola { get; set; } = null!;

    public virtual Uzytkownicy Uzytkownik { get; set; } = null!;
}
