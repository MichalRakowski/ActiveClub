using ActiveClub.Shared.DTOs.Common;

namespace ActiveClub.Shared.DTOs.Platnosci
{
    public class PlatnoscFormDataDto
    {
        public List<LookupItemDto> Zamowienia { get; set; } = new();
        public List<LookupItemDto> MetodyPlatnosci { get; set; } = new();
        public List<LookupItemDto> Statusy { get; set; } = new();
        public List<LookupItemDto> Waluty { get; set; } = new();
    }
}