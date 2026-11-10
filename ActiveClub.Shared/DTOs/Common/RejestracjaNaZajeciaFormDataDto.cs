using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActiveClub.Shared.DTOs.Common;

namespace ActiveClub.Shared.DTOs.RejestracjeNaZajecia
{
    public class RejestracjaNaZajeciaFormDataDto
    {
        public List<LookupItemDto> Harmonogramy { get; set; } = new();
        public List<LookupItemDto> Uzytkownicy { get; set; } = new();
        public List<LookupItemDto> Statusy { get; set; } = new();
    }
}