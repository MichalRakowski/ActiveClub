using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class RejestracjeNaZajecium
{
    public int RejestracjaId { get; set; }
    public int HarmonogramId { get; set; }
    public int UzytkownikId { get; set; }
    public string? Status { get; set; }
    public DateTime? DataRejestracji { get; set; }
    public string? Zrodlo { get; set; }

    public virtual HarmonogramZajec Harmonogram { get; set; } = null!;
    public virtual ApplicationUser Uzytkownik { get; set; } = null!;
}
