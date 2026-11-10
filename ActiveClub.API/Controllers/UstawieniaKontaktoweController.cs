using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.UstawieniaKontaktowe;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UstawieniaKontaktoweController : ControllerBase
    {
        private readonly IUstawieniaKontaktoweService _service;

        public UstawieniaKontaktoweController(IUstawieniaKontaktoweService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<UstawieniaKontaktoweDto>> Get()
        {
            var result = await _service.GetAsync();
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(
            [FromBody] UpdateUstawieniaKontaktoweDto dto)
        {
            var result = await _service.UpdateAsync(dto);

            if (!result)
                return BadRequest("Nie udało się zaktualizować danych kontaktowych.");

            return Ok("Zaktualizowano dane kontaktowe.");
        }
    }
}