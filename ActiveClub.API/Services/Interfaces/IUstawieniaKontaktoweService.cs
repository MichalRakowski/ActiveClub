using ActiveClub.Shared.DTOs.UstawieniaKontaktowe;

namespace ActiveClub.API.Services.Interfaces
{
    public interface IUstawieniaKontaktoweService
    {
        Task<UstawieniaKontaktoweDto> GetAsync();
        Task<bool> UpdateAsync(UpdateUstawieniaKontaktoweDto dto);
    }
}