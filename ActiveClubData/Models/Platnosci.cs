using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class Platnosci
{
    public int PlatnoscId { get; set; }
    public int ZamowienieId { get; set; }
    public int MetodaPlatnosciId { get; set; }
    public decimal? Kwota { get; set; }
    public string? Waluta { get; set; }
    public string? Status { get; set; }
    public string? Provider { get; set; }
    public string? ProviderTransactionId { get; set; }
    public DateTime? DataPlatnosci { get; set; }
    public DateTime? DataUtworzenia { get; set; }
    public string? Uwagi { get; set; }

    public virtual MetodyPlatnosci MetodaPlatnosci { get; set; } = null!;
    public virtual Zamowienium Zamowienie { get; set; } = null!;
}