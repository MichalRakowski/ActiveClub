namespace ActiveClub.PortalWWW.Models
{
    public class RejestracjaViewModel
    {
        public string Imie { get; set; } = string.Empty;

        public string Nazwisko { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Telefon { get; set; }

        public string Haslo { get; set; } = string.Empty;

        public string PowtorzHaslo { get; set; } = string.Empty;

        public bool AkceptujeRegulamin { get; set; }
    }
}