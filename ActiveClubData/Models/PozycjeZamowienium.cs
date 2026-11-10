using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class PozycjeZamowienium
{
    public int PozycjaId { get; set; }

    public int ZamowienieId { get; set; }

    public string Typ { get; set; } = null!;

    public int? RefId { get; set; }

    public string Nazwa { get; set; } = null!;

    public string? Opis { get; set; }

    public int Ilosc { get; set; }

    public decimal CenaJedn { get; set; }

    public decimal KwotaBrutto { get; set; }

    public virtual Zamowienium Zamowienie { get; set; } = null!;
}
