using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.GodzinyOtwarcia;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GodzinyOtwarciaController : ControllerBase
    {
        private readonly IGodzinyOtwarciaService _service;

        public GodzinyOtwarciaController(IGodzinyOtwarciaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<GodzinyOtwarciaDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GodzinyOtwarciaDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("klub/{klubId}")]
        public async Task<ActionResult<List<GodzinyOtwarciaDto>>> GetByKlubId(int klubId)
        {
            var result = await _service.GetByKlubIdAsync(klubId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
     [FromBody] CreateGodzinyOtwarciaDto dto)
        {
            if (dto == null)
                return BadRequest("DTO jest null.");

            var result = await _service.CreateAsync(dto);

            if (!result)
                return BadRequest("Nie udało się dodać godzin otwarcia.");

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
    int id,
    [FromBody] UpdateGodzinyOtwarciaDto dto)
        {
            if (dto == null)
                return BadRequest("DTO jest null.");

            var result = await _service.UpdateAsync(id, dto);

            if (!result)
                return NotFound();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result)
                return NotFound();

            return Ok();
        }
    }
}