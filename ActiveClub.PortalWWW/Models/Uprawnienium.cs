using System;
using System.Collections.Generic;

namespace ActiveClub.PortalWWW.Models;

public partial class Uprawnienium
{
    public int UprawnienieId { get; set; }

    public string Nazwa { get; set; } = null!;

    public virtual ICollection<Role> Rolas { get; set; } = new List<Role>();
}
