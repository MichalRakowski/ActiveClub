using ActiveClub.Shared.DTOs.Common;

namespace ActiveClub.Shared.DTOs.OfertyKlubuCzlonkostwa
{
    public class OfertaKlubuCzlonkostwoFormDataDto
    {
        public List<LookupItemDto> Kluby { get; set; } = new();
        public List<LookupItemDto> RodzajeCzlonkostwa { get; set; } = new();
    }
}