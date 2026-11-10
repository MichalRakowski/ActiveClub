using ActiveClub.Shared.DTOs.Sale;

namespace ActiveClub.API.Services.Interfaces
{
    public interface ISaleService
    {
        Task<List<SalaDto>> GetAllAsync();
        Task<SalaDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateSalaDto dto);
        Task<bool> UpdateAsync(int id, UpdateSalaDto dto);
        Task<bool> DeleteAsync(int id);
    }
}