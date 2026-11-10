using System;
using System.Collections.Generic;

namespace ActiveClub.PortalWWW.Models;

public partial class Role
{
    public int RolaId { get; set; }

    public string Nazwa { get; set; } = null!;

    public virtual ICollection<Uprawnienium> Uprawnienies { get; set; } = new List<Uprawnienium>();

    public virtual ICollection<Uzytkownicy> Uzytkowniks { get; set; } = new List<Uzytkownicy>();
}
