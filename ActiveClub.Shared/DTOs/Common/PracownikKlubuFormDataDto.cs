using ActiveClub.Shared.DTOs.Common;

namespace ActiveClub.Shared.DTOs.PracownicyKlubu
{
    public class PracownikKlubuFormDataDto
    {
        public List<LookupItemDto> Uzytkownicy { get; set; } = new();
        public List<LookupItemDto> Kluby { get; set; } = new();
        public List<LookupItemDto> Stanowiska { get; set; } = new();
    }
}