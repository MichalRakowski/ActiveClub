using System;
using System.Collections.Generic;

namespace ActiveClubData.Models;

public partial class Role
{
    public int RolaId { get; set; }

    public string Nazwa { get; set; } = null!;

    public string? Opis { get; set; }

    public virtual ICollection<UzytkownikRole> UzytkownikRoles { get; set; } = new List<UzytkownikRole>();

    public virtual ICollection<Uprawnienium> Uprawnienies { get; set; } = new List<Uprawnienium>();
}
