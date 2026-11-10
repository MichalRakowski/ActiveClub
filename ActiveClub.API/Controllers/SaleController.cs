using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Sale;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _service;

        public SaleController(ISaleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<SalaDto>>> GetAll()
        {
            try
            {
                var items = await _service.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET ALL Sale:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalaDto>> GetById(int id)
        {
            try
            {
                var item = await _service.GetByIdAsync(id);

                if (item == null)
                    return NotFound();

                return Ok(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET BY ID Sale:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSalaDto dto)
        {
            try
            {
                Console.WriteLine($"API CREATE Sala -> KlubId: {dto?.KlubId}, Nazwa: '{dto?.Nazwa}'");

                if (dto == null)
                    return BadRequest("DTO jest null.");

                var created = await _service.CreateAsync(dto);

                if (!created)
                    return BadRequest("Nie udało się dodać sali.");

                return Ok("Dodano salę.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD CREATE Sala:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSalaDto dto)
        {
            try
            {
                Console.WriteLine($"API UPDATE Sala -> ID: {id}, KlubId: {dto?.KlubId}, Nazwa: '{dto?.Nazwa}'");

                if (dto == null)
                    return BadRequest("DTO jest null.");

                var updated = await _service.UpdateAsync(id, dto);

                if (!updated)
                    return NotFound("Nie znaleziono sali lub klubu.");

                return Ok("Zaktualizowano salę.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD UPDATE Sala:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);

                if (!deleted)
                    return NotFound("Nie znaleziono sali.");

                return Ok("Usunięto salę.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD DELETE Sala:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}