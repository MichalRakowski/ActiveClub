using ActiveClub.Shared.DTOs.Stanowiska;

namespace ActiveClub.API.Services.Interfaces
{
    public interface IStanowiskaService
    {
        Task<List<StanowiskoDto>> GetAllAsync();
        Task<StanowiskoDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateStanowiskoDto dto);
        Task<bool> UpdateAsync(int id, UpdateStanowiskoDto dto);
        Task<bool> DeleteAsync(int id);
    }
}