using ActiveClub.Shared.DTOs.DostepnoscTrenera;

namespace ActiveClub.API.Services.Interfaces
{
    public interface IDostepnoscTreneraService
    {
        Task<IEnumerable<DostepnoscTreneraDto>> GetAllAsync();
        Task<IEnumerable<DostepnoscTreneraDto>> GetByTrenerIdAsync(int trenerId);
        Task<DostepnoscTreneraDto?> GetByIdAsync(int dostepnoscId);
        Task<DostepnoscTreneraDto> CreateAsync(CreateDostepnoscTreneraDto dto);
        Task<bool> UpdateAsync(int dostepnoscId, UpdateDostepnoscTreneraDto dto);
        Task<bool> DeleteAsync(int dostepnoscId);
    }
}