using ActiveClub.Shared.DTOs.Zamowienia;

namespace ActiveClub.API.Services.Interfaces
{
    public interface IZamowieniaService
    {
        Task<List<ZamowienieDto>> GetAllAsync();
        Task<ZamowienieDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateZamowienieDto dto);
        Task<bool> UpdateAsync(int id, UpdateZamowienieDto dto);
        Task<bool> DeleteAsync(int id);
        Task<ZamowienieFormDataDto> GetFormDataAsync();
    }
}