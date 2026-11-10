using System;
using System.Collections.Generic;

namespace ActiveClub.PortalWWW.Models;

public partial class LogiSystemowe
{
    public int LogId { get; set; }

    public string? Opis { get; set; }

    public DateTime? Data { get; set; }
}
