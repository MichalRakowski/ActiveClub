using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class Trenerzy
{
    public int TrenerId { get; set; }

    public int? UzytkownikId { get; set; }

    public string Imie { get; set; } = null!;

    public string Nazwisko { get; set; } = null!;

    public string? Bio { get; set; }

    public string? SpecjalizacjaOpis { get; set; }

    public decimal? StawkaZaGodzine { get; set; }

    public bool CzyAktywny { get; set; }

    public DateTime DataUtworzenia { get; set; }

    public virtual ICollection<DostepnoscTrenera> DostepnoscTreneras { get; set; } = new List<DostepnoscTrenera>();

    public virtual ICollection<HarmonogramZajec> HarmonogramZajecs { get; set; } = new List<HarmonogramZajec>();

    public virtual ICollection<RezerwacjeTrenera> RezerwacjeTreneras { get; set; } = new List<RezerwacjeTrenera>();

    public virtual ICollection<TrenerKlub> TrenerKlubs { get; set; } = new List<TrenerKlub>();

    public virtual ApplicationUser? Uzytkownik { get; set; }
   
}
