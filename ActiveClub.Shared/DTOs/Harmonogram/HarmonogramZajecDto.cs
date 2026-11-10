using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveClub.Shared.DTOs.Harmonogram;

public class HarmonogramZajecDto
{
    public int HarmonogramId { get; set; }
    public int KlubId { get; set; }
    public int RodzajZajecId { get; set; }
    public int? SalaId { get; set; }
    public int? TrenerId { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public int? LimitMiejsc { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool CzyWidoczneOnline { get; set; }
    public DateTime DataUtworzenia { get; set; }
    public int? UtworzylUzytkownikId { get; set; }
}