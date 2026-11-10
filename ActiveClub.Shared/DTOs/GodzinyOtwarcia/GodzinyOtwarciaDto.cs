using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveClub.Shared.DTOs.GodzinyOtwarcia
{
    public class GodzinyOtwarciaDto
    {
        public int GodzinyOtwarciaId { get; set; }
        public int KlubId { get; set; }
        public byte DzienTygodnia { get; set; }
        public string? GodzinaOd { get; set; }
        public string? GodzinaDo { get; set; }
        public bool CzyZamkniete { get; set; }
    }
}