using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class Zamowienium
{
    public int ZamowienieId { get; set; }

    public string NumerZamowienia { get; set; } = null!;

    public int UzytkownikId { get; set; }

    public int? KlubId { get; set; }

    public string Status { get; set; } = null!;

    public decimal SumaBrutto { get; set; }

    public decimal KwotaRabatu { get; set; }

    public string Waluta { get; set; } = null!;

    public int? MetodaPlatnosciId { get; set; }

    public int? RabatId { get; set; }

    public string TypKanalu { get; set; } = null!;

    public DateTime DataUtworzenia { get; set; }

    public DateTime? DataOplacenia { get; set; }

    public int? UtworzylUzytkownikId { get; set; }

    public virtual ICollection<CzlonkostwaUzytkownika> CzlonkostwaUzytkownikas { get; set; } = new List<CzlonkostwaUzytkownika>();

    public virtual Kluby? Klub { get; set; }

    public virtual MetodyPlatnosci? MetodaPlatnosci { get; set; }

    public virtual ICollection<Platnosci> Platnoscis { get; set; } = new List<Platnosci>();

    public virtual ICollection<PozycjeZamowienium> PozycjeZamowienia { get; set; } = new List<PozycjeZamowienium>();

    public virtual Rabaty? Rabat { get; set; }

    public virtual ApplicationUser Uzytkownik { get; set; } = null!;
}
