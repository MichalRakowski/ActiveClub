namespace ActiveClubData.Models
{
    public class WejscieKlubowe
    {
        public int WejscieKluboweId { get; set; }

        public int CzlonkostwoId { get; set; }

        public DateTime DataWejscia { get; set; } = DateTime.Now;

        public int? KlubId { get; set; }

        public int? ZarejestrowalUzytkownikId { get; set; }

        public string? Uwagi { get; set; }
    }
}