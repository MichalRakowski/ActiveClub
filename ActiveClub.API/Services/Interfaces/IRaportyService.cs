using ActiveClub.Shared.DTOs.Raporty;

namespace ActiveClub.API.Services.Interfaces
{
    public interface IRaportyService
    {
        Task<RaportFormDataDto> GetFormDataAsync();
        Task<List<RaportDefinicjaDto>> GetDefinitionsAsync();
        Task<List<RaportWygenerowanyDto>> GetGeneratedAsync();
        Task<GenerateRaportResultDto> GenerateAsync(GenerateRaportDto dto);
    }
}