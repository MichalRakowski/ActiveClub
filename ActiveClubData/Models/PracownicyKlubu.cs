using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class PracownicyKlubu
{
    public int PracownikKlubId { get; set; }
    public int UzytkownikId { get; set; }
    public int KlubId { get; set; }
    public int StanowiskoId { get; set; }
    public DateOnly? DataOd { get; set; }
    public DateOnly? DataDo { get; set; }
    public bool? CzyAktywny { get; set; }
    public string? Uwagi { get; set; }

    public virtual Kluby Klub { get; set; } = null!;
    public virtual Stanowiska Stanowisko { get; set; } = null!;
    public virtual ApplicationUser Uzytkownik { get; set; } = null!;
}