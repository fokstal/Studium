using api.Data;
using api.Model;
using api.Model.DTO;
using api.Services.DataServices;
using Microsoft.AspNetCore.Mvc;

using static api.Services.PictureWorker;

namespace api.Controllers
{
    [Route("passport")]
    [ApiController]
    public class PassportController(AppDbContext db) : ControllerBase
    {
        private readonly PassportService _passportService = new(db);
        private readonly PersonService _personService = new(db);

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Passport>>> GetListAsync() => Ok(await _passportService.GetListAsync());

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Passport>> GetAsync(int id)
        {
            if (id < 1) return BadRequest();

            Passport? passport = await _passportService.GetAsync(id);

            if (passport is null) return NotFound();

            return Ok(passport);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Passport>> CreateAsync([FromForm] PassportDTO passportDTO)
        {
            if (passportDTO.Scan is null) return BadRequest();

            Person? person = await _personService.GetAsync(passportDTO.PersonId);

            if (person is null) return NotFound("Person is null!");

            await _passportService.AddAsync(new()
            {
                ScanFileName = await UploadPassportScanAsync(passportDTO.Scan),
                PersonId = person.Id,
            });

            return Created("Passport", passportDTO);
        }

        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] PassportDTO passportDTO)
        {
            if (id < 1) return BadRequest();

            if (passportDTO.Scan is null) return BadRequest();

            Passport? passportToUpdate = await _passportService.GetAsync(id);

            if (passportToUpdate is null) return NotFound();

            Person? person = await _personService.GetAsync(passportToUpdate.PersonId);

            if (person is null) return NotFound("Person is null!");

            System.IO.File.Delete($"{picturesFolderPath}/Passport/{passportToUpdate.ScanFileName}");

            await _passportService.UpdateAsync(passportToUpdate, passportDTO);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            if (id < 1) return BadRequest();

            Passport? passportToRemove = await _passportService.GetAsync(id);

            if (passportToRemove is null) return NotFound();

            System.IO.File.Delete($"{picturesFolderPath}/Passport/{passportToRemove.ScanFileName}");

            await _passportService.RemoveAsync(passportToRemove);

            return NoContent();
        }
    }
}
