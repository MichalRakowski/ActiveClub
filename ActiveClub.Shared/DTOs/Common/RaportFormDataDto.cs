using ActiveClub.Shared.DTOs.Common;

namespace ActiveClub.Shared.DTOs.Raporty
{
    public class RaportFormDataDto
    {
        public List<LookupItemDto> DefinicjeRaportow { get; set; } = new();
        public List<LookupItemDto> Kluby { get; set; } = new();
        public List<LookupItemDto> Trenerzy { get; set; } = new();
        public List<LookupItemDto> Formaty { get; set; } = new();
    }
}