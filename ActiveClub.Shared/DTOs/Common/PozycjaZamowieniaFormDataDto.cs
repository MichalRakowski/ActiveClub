using ActiveClub.Shared.DTOs.Common;

namespace ActiveClub.Shared.DTOs.PozycjeZamowienia
{
    public class PozycjaZamowieniaFormDataDto
    {
        public List<LookupItemDto> Zamowienia { get; set; } = new();
        public List<LookupItemDto> Typy { get; set; } = new();
    }
}