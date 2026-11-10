using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class RaportyWygenerowane
{
    public int RaportWygenerowanyId { get; set; }

    public int RaportDefinicjaId { get; set; }

    public int? WygenerowalUzytkownikId { get; set; }

    public string Format { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? PlikUrl { get; set; }

    public string? ParametryJson { get; set; }

    public DateTime? DataGeneracji { get; set; }
    public virtual RaportyDefinicje RaportDefinicja { get; set; } = null!;
    public virtual ApplicationUser? WygenerowalUzytkownik { get; set; }
}
