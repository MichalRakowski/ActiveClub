using ActiveClub.Shared.DTOs.PozycjeZamowienia;

namespace ActiveClub.API.Services.Interfaces
{
    public interface IPozycjeZamowieniaService
    {
        Task<List<PozycjaZamowieniaDto>> GetAllAsync();
        Task<PozycjaZamowieniaDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreatePozycjaZamowieniaDto dto);
        Task<bool> UpdateAsync(int id, UpdatePozycjaZamowieniaDto dto);
        Task<bool> DeleteAsync(int id);
        Task<PozycjaZamowieniaFormDataDto> GetFormDataAsync();
    }
}