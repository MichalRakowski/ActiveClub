using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class Stanowiska
{
    public int StanowiskoId { get; set; }

    public string Nazwa { get; set; } = null!;

    public string? Opis { get; set; }

    public bool CzyAktywne { get; set; }

    public virtual ICollection<PracownicyKlubu> PracownicyKlubus { get; set; } = new List<PracownicyKlubu>();
}
