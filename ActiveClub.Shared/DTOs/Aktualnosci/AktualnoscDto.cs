using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveClub.Shared.DTOs.Aktualnosci;

public class AktualnoscDto
{
    public int AktualnoscId { get; set; }
    public string Tytul { get; set; } = "";
    public string? Tresc { get; set; }
    public int? KlubId { get; set; }
    public bool? CzyWidoczna { get; set; }
    public DateTime? DataPublikacji { get; set; }
    public DateTime? DataDodania { get; set; }
    public int? DodanePrzezUzytkownikId { get; set; }
}