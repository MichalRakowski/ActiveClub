using ActiveClub.Shared.DTOs.Common;

namespace ActiveClub.Shared.DTOs.CzlonkostwaUzytkownika
{
    public class CzlonkostwoUzytkownikaFormDataDto
    {
        public List<LookupItemDto> Uzytkownicy { get; set; } = new();
        public List<LookupItemDto> OfertyKlubuCzlonkostwa { get; set; } = new();
        public List<LookupItemDto> RodzajeCzlonkostwa { get; set; } = new();
        public List<LookupItemDto> Kluby { get; set; } = new();
        public List<LookupItemDto> Zamowienia { get; set; } = new();
        public List<LookupItemDto> NadajacyUzytkownicy { get; set; } = new();
        public List<LookupItemDto> Statusy { get; set; } = new();
    }
}