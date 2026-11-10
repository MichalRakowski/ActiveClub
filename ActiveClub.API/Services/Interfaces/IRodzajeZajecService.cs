using ActiveClub.Shared.DTOs.RodzajeZajec;

namespace ActiveClub.API.Services.Interfaces
{
    public interface IRodzajeZajecService
    {
        Task<List<RodzajZajecDto>> GetAllAsync();
        Task<RodzajZajecDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateRodzajZajecDto dto);
        Task<bool> UpdateAsync(int id, UpdateRodzajZajecDto dto);
        Task<bool> DeleteAsync(int id);
    }
}