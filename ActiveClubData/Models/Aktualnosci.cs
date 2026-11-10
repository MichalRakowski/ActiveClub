using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class Aktualnosci
{
    public int AktualnoscId { get; set; }
    public string Tytul { get; set; } = null!;
    public string? Tresc { get; set; }
    public int? KlubId { get; set; }
    public bool? CzyWidoczna { get; set; }
    public DateTime? DataPublikacji { get; set; }
    public DateTime? DataDodania { get; set; }
    public int? DodanePrzezUzytkownikId { get; set; }

    public virtual Kluby? Klub { get; set; }
    public virtual ApplicationUser? DodanePrzezUzytkownik { get; set; }
}