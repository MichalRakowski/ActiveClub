namespace ActiveClubData.Models
{
    public class UstawieniaKontaktowe
    {
        public int UstawieniaKontaktoweId { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Telefon { get; set; } = string.Empty;

        public string Adres { get; set; } = string.Empty;

        public string GodzinyKontaktu { get; set; } = string.Empty;

        public DateTime DataModyfikacji { get; set; } = DateTime.Now;
    }
}