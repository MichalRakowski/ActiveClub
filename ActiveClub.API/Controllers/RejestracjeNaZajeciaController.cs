using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.RejestracjeNaZajecia;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RejestracjeNaZajeciaController : ControllerBase
    {
        private readonly IRejestracjeNaZajeciaService _service;

        public RejestracjeNaZajeciaController(IRejestracjeNaZajeciaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<RejestracjaNaZajeciaDto>>> GetAll()
        {
            try
            {
                var items = await _service.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET ALL RejestracjeNaZajecia:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }
        [HttpGet("uzytkownik/{uzytkownikId:int}")]
        public async Task<ActionResult<List<RejestracjaNaZajeciaDto>>> GetByUzytkownik(int uzytkownikId)
        {
            var result = await _service.GetByUzytkownikIdAsync(uzytkownikId);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<RejestracjaNaZajeciaDto>> GetById(int id)
        {
            try
            {
                var item = await _service.GetByIdAsync(id);
                if (item == null) return NotFound();
                return Ok(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET BY ID RejestracjeNaZajecia:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet("form-data")]
        public async Task<ActionResult<RejestracjaNaZajeciaFormDataDto>> GetFormData()
        {
            try
            {
                var data = await _service.GetFormDataAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET FORM DATA RejestracjeNaZajecia:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRejestracjaNaZajeciaDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("DTO jest null.");

                var created = await _service.CreateAsync(dto);

                if (!created)
                    return BadRequest("Nie udało się dodać rezerwacji.");

                return Ok("Dodano rezerwację.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD CREATE RejestracjeNaZajecia:");
                Console.WriteLine(ex.ToString());

                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRejestracjaNaZajeciaDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("DTO jest null.");

                var updated = await _service.UpdateAsync(id, dto);

                if (!updated)
                    return NotFound("Nie znaleziono rezerwacji.");

                return Ok("Zaktualizowano rezerwację.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD UPDATE RejestracjeNaZajecia:");
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
                if (!deleted) return NotFound("Nie znaleziono rezerwacji.");

                return Ok("Usunięto rezerwację.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD DELETE RejestracjeNaZajecia:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}