using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Stanowiska;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StanowiskaController : ControllerBase
    {
        private readonly IStanowiskaService _service;

        public StanowiskaController(IStanowiskaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<StanowiskoDto>>> GetAll()
        {
            try
            {
                var items = await _service.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET ALL Stanowiska:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StanowiskoDto>> GetById(int id)
        {
            try
            {
                var item = await _service.GetByIdAsync(id);
                if (item == null) return NotFound();
                return Ok(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET BY ID Stanowiska:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(
     [FromBody] CreateStanowiskoDto dto)
        {
            try
            {
                Console.WriteLine(
                    $"API CREATE Stanowisko -> Nazwa: '{dto?.Nazwa}'");

                if (dto == null)
                    return BadRequest("DTO jest null.");

                var created = await _service.CreateAsync(dto);

                if (!created)
                    return BadRequest("Nie udało się dodać stanowiska.");

                return Ok("Dodano stanowisko.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD CREATE Stanowiska:");
                Console.WriteLine(ex.ToString());

                return StatusCode(
                    500,
                    ex.InnerException?.Message ?? ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateStanowiskoDto dto)
        {
            try
            {
                Console.WriteLine(
                    $"API UPDATE Stanowisko -> ID: {id}, Nazwa: '{dto?.Nazwa}'");

                if (dto == null)
                    return BadRequest("DTO jest null.");

                var updated = await _service.UpdateAsync(id, dto);

                if (!updated)
                    return NotFound("Nie znaleziono stanowiska.");

                return Ok("Zaktualizowano stanowisko.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD UPDATE Stanowiska:");
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
                if (!deleted) return NotFound("Nie znaleziono stanowiska.");

                return Ok("Usunięto stanowisko.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD DELETE Stanowiska:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}