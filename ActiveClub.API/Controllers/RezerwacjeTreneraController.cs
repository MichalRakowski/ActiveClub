using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.RezerwacjeTrenera;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RezerwacjeTreneraController : ControllerBase
    {
        private readonly IRezerwacjeTreneraService _service;

        public RezerwacjeTreneraController(IRezerwacjeTreneraService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RezerwacjaTreneraDto>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RezerwacjaTreneraDto>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpGet("uzytkownik/{uzytkownikId}")]
        public async Task<ActionResult<IEnumerable<RezerwacjaTreneraDto>>> GetByUzytkownikId(int uzytkownikId)
        {
            var items = await _service.GetByUzytkownikIdAsync(uzytkownikId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<RezerwacjaTreneraDto>> Create([FromBody] CreateRezerwacjaTreneraDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.RezerwacjaId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRezerwacjaTreneraDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}