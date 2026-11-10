using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.MetodyPlatnosci;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetodyPlatnosciController : ControllerBase
    {
        private readonly IMetodyPlatnosciService _service;

        public MetodyPlatnosciController(IMetodyPlatnosciService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<MetodaPlatnosciDto>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MetodaPlatnosciDto>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateMetodaPlatnosciDto dto)
        {
            Console.WriteLine(
                $"API CREATE -> Nazwa: '{dto?.Nazwa}', CzyAktywna: {dto?.CzyAktywna}");

            if (dto == null)
                return BadRequest("DTO jest null.");

            var created = await _service.CreateAsync(dto);

            if (!created)
                return BadRequest("Nie udało się dodać metody płatności.");

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
     int id,
     [FromBody] UpdateMetodaPlatnosciDto dto)
        {
            Console.WriteLine(
                $"API UPDATE -> ID: {id}, Nazwa: '{dto?.Nazwa}', CzyAktywna: {dto?.CzyAktywna}");

            if (dto == null)
                return BadRequest("DTO jest null.");

            var updated = await _service.UpdateAsync(id, dto);

            if (!updated)
                return NotFound("Nie znaleziono metody płatności.");

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound("Nie znaleziono metody płatności.");

            return Ok();
        }
    }
}