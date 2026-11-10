using ActiveClub.Shared.DTOs.HarmonogramZajec;

namespace ActiveClub.API.Services.Interfaces
{
    public interface IHarmonogramZajecService
    {
        Task<List<HarmonogramZajecDto>> GetAllAsync();
        Task<HarmonogramZajecDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateHarmonogramZajecDto dto);
        Task<bool> UpdateAsync(int id, UpdateHarmonogramZajecDto dto);
        Task<bool> DeleteAsync(int id);

        Task<List<ActiveClub.Shared.DTOs.Harmonogram.HarmonogramZajecListDto>> GetPublicznyGrafikAsync(
            int? klubId,
            DateTime dzien);
    }
}