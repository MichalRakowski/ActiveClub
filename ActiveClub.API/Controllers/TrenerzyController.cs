using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Trenerzy;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrenerzyController : ControllerBase
    {
        private readonly ITrenerzyService _trenerzyService;

        public TrenerzyController(ITrenerzyService trenerzyService)
        {
            _trenerzyService = trenerzyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrenerDto>>> GetAll()
        {
            var trenerzy = await _trenerzyService.GetAllAsync();
            return Ok(trenerzy);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TrenerDto>> GetById(int id)
        {
            var trener = await _trenerzyService.GetByIdAsync(id);
            if (trener == null)
                return NotFound();

            return Ok(trener);
        }

        [HttpPost]
        public async Task<ActionResult<TrenerDto>> Create([FromBody] CreateTrenerDto dto)
        {
            var created = await _trenerzyService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.TrenerId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTrenerDto dto)
        {
            var updated = await _trenerzyService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _trenerzyService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}