using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class RezerwacjeTrenera
{
    public int RezerwacjaId { get; set; }
    public int TrenerId { get; set; }
    public int UzytkownikId { get; set; }
    public int? KlubId { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public string? Status { get; set; }
    public decimal? Cena { get; set; }
    public DateTime? DataUtworzenia { get; set; }

    public virtual Kluby? Klub { get; set; }
    public virtual Trenerzy Trener { get; set; } = null!;
    public virtual ApplicationUser Uzytkownik { get; set; } = null!;
}