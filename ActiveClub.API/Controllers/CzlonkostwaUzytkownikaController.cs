using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Common;
using ActiveClub.Shared.DTOs.CzlonkostwaUzytkownika;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CzlonkostwaUzytkownikaController : ControllerBase
    {
        private readonly ICzlonkostwaUzytkownikaService _service;

        public CzlonkostwaUzytkownikaController(ICzlonkostwaUzytkownikaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<CzlonkostwoUzytkownikaDto>>> GetAll()
        {
            try
            {
                var items = await _service.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET ALL CzlonkostwaUzytkownika:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CzlonkostwoUzytkownikaDto>> GetById(int id)
        {
            try
            {
                var item = await _service.GetByIdAsync(id);
                if (item == null)
                    return NotFound("Nie znaleziono członkostwa.");

                return Ok(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET BY ID CzlonkostwaUzytkownika:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet("form-data")]
        public async Task<ActionResult<CzlonkostwoUzytkownikaFormDataDto>> GetFormData()
        {
            try
            {
                var data = await _service.GetFormDataAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET FORM-DATA CzlonkostwaUzytkownika:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet("pracownicy-klubu/{klubId}")]
        public async Task<ActionResult<List<LookupItemDto>>> GetPracownicyKlubu(int klubId)
        {
            try
            {
                var items = await _service.GetPracownicyKlubuAsync(klubId);
                return Ok(items);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET PRACOWNICY-KLUBU CzlonkostwaUzytkownika:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(
     [FromBody] CreateCzlonkostwoUzytkownikaDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("DTO jest null.");

                if (dto.StartAt.HasValue &&
                    dto.EndAt.HasValue &&
                    dto.StartAt > dto.EndAt)
                {
                    return BadRequest(
                        "StartAt nie może być większa od EndAt.");
                }

                var created = await _service.CreateAsync(dto);

                if (!created)
                    return BadRequest(
                        "Nie udało się dodać członkostwa.");

                return Ok("Dodano członkostwo.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "BŁĄD CREATE CzlonkostwaUzytkownika:");
                Console.WriteLine(ex.ToString());

                return StatusCode(
                    500,
                    ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
     int id,
     [FromBody] UpdateCzlonkostwoUzytkownikaDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("DTO jest null.");

                if (dto.StartAt.HasValue &&
                    dto.EndAt.HasValue &&
                    dto.StartAt > dto.EndAt)
                {
                    return BadRequest(
                        "StartAt nie może być większa od EndAt.");
                }

                var updated = await _service.UpdateAsync(id, dto);

                if (!updated)
                    return NotFound(
                        "Nie znaleziono członkostwa.");

                return Ok("Zaktualizowano członkostwo.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "BŁĄD UPDATE CzlonkostwaUzytkownika:");
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
                if (!deleted)
                    return NotFound("Nie znaleziono członkostwa.");

                return Ok("Usunięto członkostwo.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD DELETE CzlonkostwaUzytkownika:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }
        [HttpPost("zakup")]
        public async Task<IActionResult> Zakup(
    [FromBody] ZakupKarnetuDto dto)
        {
            var result = await _service.ZakupKarnetuAsync(
                dto.UzytkownikId,
                dto.KlubId,
                dto.RodzajCzlonkostwaId);

            if (!result)
                return BadRequest(
                    "Nie udało się zapisać karnetu.");

            return Ok();
        }
        [HttpGet("uzytkownik/{uzytkownikId:int}")]
        public async Task<ActionResult<List<CzlonkostwoUzytkownikaDto>>> GetByUzytkownik(int uzytkownikId)
        {
            var result = await _service.GetByUzytkownikIdAsync(uzytkownikId);
            return Ok(result);
        }
    
    [HttpPost("{id}/wejscie")]
        public async Task<IActionResult> DodajWejscie(int id)
        {
            try
            {
                var result = await _service.DodajWejscieAsync(id);

                if (!result)
                    return BadRequest(
                        "Brak dostępnych wejść lub nie znaleziono członkostwa.");

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return StatusCode(
                    500,
                    ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPost("{id}/cofnij-wejscie")]
        public async Task<IActionResult> CofnijWejscie(int id)
        {
            try
            {
                var result = await _service.CofnijWejscieAsync(id);

                if (!result)
                    return BadRequest(
                        "Nie można cofnąć wejścia.");

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return StatusCode(
                    500,
                    ex.InnerException?.Message ?? ex.Message);
            }
        }
        [HttpPut("{id}/anuluj")]
        public async Task<IActionResult> Anuluj(int id)
        {
            try
            {
                var item = await _service.GetByIdAsync(id);

                if (item == null)
                    return NotFound();

                var dto = new UpdateCzlonkostwoUzytkownikaDto
                {
                    UzytkownikId = item.UzytkownikId,
                    KlubId = item.KlubId,
                    RodzajCzlonkostwaId = item.RodzajCzlonkostwaId,
                    Status = "ANULOWANY",
                    StartAt = item.StartAt,
                    EndAt = item.EndAt,
                    CenaZakupu = item.CenaZakupu
                };

                var result = await _service.UpdateAsync(id, dto);

                if (!result)
                    return BadRequest();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    ex.InnerException?.Message ?? ex.Message);
            }
        }
        [HttpPost("anuluj-przeterminowane")]
        public async Task<ActionResult<int>> AnulujPrzeterminowane()
        {
            var count = await _service.AnulujPrzeterminowaneAsync();
            return Ok(count);
        }
    }
}
