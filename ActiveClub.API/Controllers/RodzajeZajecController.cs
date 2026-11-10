using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.RodzajeZajec;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RodzajeZajecController : ControllerBase
    {
        private readonly IRodzajeZajecService _service;

        public RodzajeZajecController(IRodzajeZajecService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<RodzajZajecDto>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RodzajZajecDto>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRodzajZajecDto dto)
        {
            if (dto == null)
                return BadRequest("DTO jest null.");

            var created = await _service.CreateAsync(dto);

            if (!created)
                return BadRequest("Nie udało się dodać.");

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRodzajZajecDto dto)
        {
            if (dto == null)
                return BadRequest("DTO jest null.");

            var updated = await _service.UpdateAsync(id, dto);

            if (!updated)
                return NotFound();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return Ok();
        }
    }
}