using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveClub.Shared.DTOs.Rabaty
{
    public class RabatDto
    {
        public int RabatId { get; set; }
        public string Kod { get; set; } = string.Empty;
        public string Typ { get; set; } = string.Empty;
        public decimal Wartosc { get; set; }
        public DateOnly? DataOd { get; set; }
        public DateOnly? DataDo { get; set; }
        public decimal? MinimalnaKwotaZamowienia { get; set; }
        public int? MaksLiczbaUzyc { get; set; }
        public bool CzyAktywny { get; set; }
    }
}