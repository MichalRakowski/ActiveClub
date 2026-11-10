using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class Uprawnienium
{
    public int UprawnienieId { get; set; }

    public string Kod { get; set; } = null!;

    public string? Opis { get; set; }

    public virtual ICollection<Role> Rolas { get; set; } = new List<Role>();
}
