using ActiveClub.Shared.DTOs.Trenerzy;

namespace ActiveClub.API.Services.Interfaces
{
    public interface ITrenerzyService
    {
        Task<IEnumerable<TrenerDto>> GetAllAsync();
        Task<TrenerDto?> GetByIdAsync(int trenerId);
        Task<TrenerDto> CreateAsync(CreateTrenerDto dto);
        Task<bool> UpdateAsync(int trenerId, UpdateTrenerDto dto);
        Task<bool> DeleteAsync(int trenerId);
    }
}