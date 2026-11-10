using ActiveClub.Shared.DTOs.Opinie;

namespace ActiveClub.API.Services.Interfaces
{
    public interface IOpinieService
    {
        Task<List<OpiniaDto>> GetAllAsync();
        Task<OpiniaDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, UpdateOpiniaDto dto);
        Task<bool> DeleteAsync(int id);
    }
}