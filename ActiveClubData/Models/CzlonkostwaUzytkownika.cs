using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class CzlonkostwaUzytkownika
{
    public int CzlonkostwoId { get; set; }
    public int UzytkownikId { get; set; }

    public int? OfertaKlubuCzlonkostwoId { get; set; }
    public int? RodzajCzlonkostwaId { get; set; }
    public int? KlubId { get; set; }

    public DateOnly? StartAt { get; set; }
    public DateOnly? EndAt { get; set; }

    public string? Status { get; set; }
    public decimal? CenaZakupu { get; set; }
    public int? ZamowienieId { get; set; }

    public int? NadanePrzezUzytkownikId { get; set; }
    public DateTime? DataUtworzenia { get; set; }
    public DateTime? DataAktywacji { get; set; }
    public DateTime? DataAnulowania { get; set; }
    public string? PowodAnulowania { get; set; }
    public int? LiczbaWejscLimit { get; set; }

    public int LiczbaWejscWykorzystanych { get; set; }
    public virtual Kluby? Klub { get; set; }
    public virtual OfertaKlubuCzlonkostwo? OfertaKlubuCzlonkostwo { get; set; }
    public virtual RodzajeCzlonkostwa? RodzajCzlonkostwa { get; set; }
    public virtual ApplicationUser Uzytkownik { get; set; } = null!;
    public virtual Zamowienium? Zamowienie { get; set; }
}