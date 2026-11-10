using ActiveClub.Shared.DTOs.Common;

namespace ActiveClub.Shared.DTOs.Zamowienia
{
    public class ZamowienieFormDataDto
    {
        public List<LookupItemDto> Uzytkownicy { get; set; } = new();
        public List<LookupItemDto> Kluby { get; set; } = new();
        public List<LookupItemDto> MetodyPlatnosci { get; set; } = new();
        public List<LookupItemDto> Rabaty { get; set; } = new();
        public List<LookupItemDto> UtworzylUzytkownicy { get; set; } = new();
    }
}