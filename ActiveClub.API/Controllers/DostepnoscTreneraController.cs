using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.DostepnoscTrenera;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DostepnoscTreneraController : ControllerBase
    {
        private readonly IDostepnoscTreneraService _service;

        public DostepnoscTreneraController(IDostepnoscTreneraService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DostepnoscTreneraDto>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DostepnoscTreneraDto>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpGet("trener/{trenerId}")]
        public async Task<ActionResult<IEnumerable<DostepnoscTreneraDto>>> GetByTrenerId(int trenerId)
        {
            var items = await _service.GetByTrenerIdAsync(trenerId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<DostepnoscTreneraDto>> Create(
     [FromBody] CreateDostepnoscTreneraDto dto)
        {
            if (dto == null)
                return BadRequest("DTO jest null.");

            if (dto.StartAt >= dto.EndAt)
            {
                return BadRequest(
                    "StartAt musi być wcześniejsze niż EndAt.");
            }

            var created = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.DostepnoscId },
                created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
     int id,
     [FromBody] UpdateDostepnoscTreneraDto dto)
        {
            if (dto == null)
                return BadRequest("DTO jest null.");

            if (dto.StartAt >= dto.EndAt)
            {
                return BadRequest(
                    "StartAt musi być wcześniejsze niż EndAt.");
            }

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