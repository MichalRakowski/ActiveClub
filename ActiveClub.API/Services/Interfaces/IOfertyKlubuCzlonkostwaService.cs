using ActiveClub.Shared.DTOs.OfertyKlubuCzlonkostwa;

namespace ActiveClub.API.Services.Interfaces
{
    public interface IOfertyKlubuCzlonkostwaService
    {
        Task<List<OfertaKlubuCzlonkostwoDto>> GetAllAsync();
        Task<OfertaKlubuCzlonkostwoDto?> GetByIdAsync(int id);
        Task<OfertaKlubuCzlonkostwoFormDataDto> GetFormDataAsync();
        Task<bool> CreateAsync(CreateOfertaKlubuCzlonkostwoDto dto);
        Task<bool> UpdateAsync(int id, UpdateOfertaKlubuCzlonkostwoDto dto);
        Task<bool> DeleteAsync(int id);
    }
}