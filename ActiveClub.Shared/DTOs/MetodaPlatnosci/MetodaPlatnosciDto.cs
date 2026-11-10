using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveClub.Shared.DTOs.MetodyPlatnosci
{
    public class MetodaPlatnosciDto
    {
        public int MetodaPlatnosciId { get; set; }
        public string Nazwa { get; set; } = string.Empty;
        public bool CzyAktywna { get; set; }
    }
}