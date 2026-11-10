using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.OfertyKlubuCzlonkostwa;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OfertyKlubuCzlonkostwaController : ControllerBase
    {
        private readonly IOfertyKlubuCzlonkostwaService _service;

        public OfertyKlubuCzlonkostwaController(IOfertyKlubuCzlonkostwaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<OfertaKlubuCzlonkostwoDto>>> GetAll()
        {
            try
            {
                var items = await _service.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET ALL OfertyKlubuCzlonkostwa:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OfertaKlubuCzlonkostwoDto>> GetById(int id)
        {
            try
            {
                var item = await _service.GetByIdAsync(id);
                if (item == null) return NotFound();
                return Ok(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET BY ID OfertyKlubuCzlonkostwa:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet("form-data")]
        public async Task<ActionResult<OfertaKlubuCzlonkostwoFormDataDto>> GetFormData()
        {
            try
            {
                var data = await _service.GetFormDataAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET FORM DATA OfertyKlubuCzlonkostwa:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(
     [FromBody] CreateOfertaKlubuCzlonkostwoDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("DTO jest null.");

                if (dto.DataOd.HasValue &&
                    dto.DataDo.HasValue &&
                    dto.DataOd > dto.DataDo)
                {
                    return BadRequest("DataOd nie może być większa od DataDo.");
                }

                var created = await _service.CreateAsync(dto);

                if (!created)
                    return BadRequest("Nie udało się dodać oferty członkostwa.");

                return Ok("Dodano ofertę członkostwa.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD CREATE OfertyKlubuCzlonkostwa:");
                Console.WriteLine(ex.ToString());

                return StatusCode(
                    500,
                    ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
     int id,
     [FromBody] UpdateOfertaKlubuCzlonkostwoDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("DTO jest null.");

                if (dto.DataOd.HasValue &&
                    dto.DataDo.HasValue &&
                    dto.DataOd > dto.DataDo)
                {
                    return BadRequest("DataOd nie może być większa od DataDo.");
                }

                var updated = await _service.UpdateAsync(id, dto);

                if (!updated)
                    return NotFound("Nie znaleziono oferty członkostwa.");

                return Ok("Zaktualizowano ofertę członkostwa.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD UPDATE OfertyKlubuCzlonkostwa:");
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
                if (!deleted) return NotFound("Nie znaleziono oferty członkostwa.");
                return Ok("Usunięto ofertę członkostwa.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD DELETE OfertyKlubuCzlonkostwa:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}