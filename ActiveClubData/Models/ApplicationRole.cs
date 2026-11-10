using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ActiveClubData.Models
{
    public class ApplicationRole : IdentityRole<int>
    {
        public string? Opis { get; set; }
        public bool CzyAktywna { get; set; }
    }
}