using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Uzytkownicy;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UzytkownicyController : ControllerBase
    {
        private readonly IUzytkownicyService _service;

        public UzytkownicyController(IUzytkownicyService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }
     
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUzytkownikDto dto)
        {
            var result = await _service.CreateAsync(dto);

            if (!result.Succeeded)
                return BadRequest(result.Errors.ToList());

            return Ok(result.User);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUzytkownikDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);

            if (!result.Succeeded)
                return BadRequest(result.Errors.ToList());

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}