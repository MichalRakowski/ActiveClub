using Microsoft.AspNetCore.Identity;

namespace ActiveClub.API.Data.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Imie { get; set; } = string.Empty;
        public string Nazwisko { get; set; } = string.Empty;
        public bool CzyAktywny { get; set; } = true;
        public int? KlubDomyslnyId { get; set; }
        public string? Uwagi { get; set; }
    }
}