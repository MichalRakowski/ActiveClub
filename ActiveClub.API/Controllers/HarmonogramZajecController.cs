using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.HarmonogramZajec;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HarmonogramZajecController : ControllerBase
    {
        private readonly IHarmonogramZajecService _service;

        public HarmonogramZajecController(IHarmonogramZajecService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<HarmonogramZajecDto>>> GetAll()
        {
            try
            {
                var items = await _service.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET ALL HarmonogramZajec:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }
        [HttpGet("publiczny")]
        public async Task<ActionResult<List<ActiveClub.Shared.DTOs.Harmonogram.HarmonogramZajecListDto>>> GetPubliczny(
    [FromQuery] int? klubId,
    [FromQuery] DateTime? dzien)
        {
            try
            {
                var result = await _service.GetPublicznyGrafikAsync(
                    klubId,
                    dzien ?? DateTime.Today);

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET PUBLICZNY HarmonogramZajec:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<HarmonogramZajecDto>> GetById(int id)
        {
            try
            {
                var item = await _service.GetByIdAsync(id);
                if (item == null) return NotFound();
                return Ok(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET BY ID HarmonogramZajec:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(
     [FromBody] CreateHarmonogramZajecDto dto)
        {
            try
            {
                Console.WriteLine(
                    $"API CREATE HarmonogramZajec -> " +
                    $"KlubId: {dto?.KlubId}, " +
                    $"RodzajZajecId: {dto?.RodzajZajecId}");

                if (dto == null)
                    return BadRequest("DTO jest null.");

                if (dto.StartAt >= dto.EndAt)
                {
                    return BadRequest(
                        "StartAt musi być wcześniejsze niż EndAt.");
                }

                var created = await _service.CreateAsync(dto);

                if (!created)
                {
                    return BadRequest(
                        "Nie udało się dodać harmonogramu zajęć.");
                }

                return Ok("Dodano harmonogram zajęć.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD CREATE HarmonogramZajec:");
                Console.WriteLine(ex.ToString());

                return StatusCode(
                    500,
                    ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
     int id,
     [FromBody] UpdateHarmonogramZajecDto dto)
        {
            try
            {
                Console.WriteLine(
                    $"API UPDATE HarmonogramZajec -> " +
                    $"ID: {id}, " +
                    $"KlubId: {dto?.KlubId}, " +
                    $"RodzajZajecId: {dto?.RodzajZajecId}");

                if (dto == null)
                    return BadRequest("DTO jest null.");

                if (dto.StartAt >= dto.EndAt)
                {
                    return BadRequest(
                        "StartAt musi być wcześniejsze niż EndAt.");
                }

                var updated = await _service.UpdateAsync(id, dto);

                if (!updated)
                {
                    return NotFound(
                        "Nie znaleziono harmonogramu zajęć.");
                }

                return Ok("Zaktualizowano harmonogram zajęć.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD UPDATE HarmonogramZajec:");
                Console.WriteLine(ex.ToString());

                return StatusCode(
                    500,
                    ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted) return NotFound("Nie znaleziono harmonogramu zajęć.");
                return Ok("Usunięto harmonogram zajęć.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD DELETE HarmonogramZajec:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}