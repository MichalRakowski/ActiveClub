using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class UzycieRabatu
{
    public int UzycieRabatuId { get; set; }

    public int RabatId { get; set; }

    public int? UzytkownikId { get; set; }

    public int? ZamowienieId { get; set; }

    public DateTime DataUzycia { get; set; }

    public virtual Rabaty Rabat { get; set; } = null!;

    public virtual Uzytkownicy? Uzytkownik { get; set; }

    public virtual Zamowienium? Zamowienie { get; set; }
}
