using ActiveClub.Shared.DTOs.Rabaty;

namespace ActiveClub.API.Services.Interfaces
{
    public interface IRabatyService
    {
        Task<List<RabatDto>> GetAllAsync();
        Task<RabatDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateRabatDto dto);
        Task<bool> UpdateAsync(int id, UpdateRabatDto dto);
        Task<bool> DeleteAsync(int id);
    }
}