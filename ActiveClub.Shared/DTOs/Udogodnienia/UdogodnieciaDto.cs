using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveClub.Shared.DTOs.Udogodnienia;

public class UdogodnienieDto
{
    public int UdogodnienieId { get; set; }
    public string Nazwa { get; set; } = string.Empty;
    public string? Opis { get; set; }
}