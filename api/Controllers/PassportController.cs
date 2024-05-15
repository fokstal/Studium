using api.Data;
using api.Models;
using api.Model.DTO;
using api.Services;
using api.Services.DataServices;
using api.Helpers.Constants;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IEnumerable<PassportEntity>>> GetListAsync() => Ok(await _passportService.GetListAsync());

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PassportEntity>> GetAsync(int id)
        {
            if (id < 1) return BadRequest();

            PassportEntity? passport = await _passportService.GetAsync(id);

            if (passport is null) return NotFound();

            return Ok(passport);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PassportDTO>> CreateAsync([FromForm] PassportDTO passportDTO)
        {
            if (passportDTO.Scan is null) return BadRequest();

            PersonEntity? person = await _personService.GetAsync(passportDTO.PersonId);

            if (person is null) return NotFound("Person is null!");

            await _passportService.AddAsync(new()
            {
                ScanFileName = await PictureWorker.UploadPassportScanAsync(passportDTO.Scan),
                PersonId = person.Id,
            });

            return Created("PassportEntity", passportDTO);
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

            PassportEntity? passportToUpdate = await _passportService.GetAsync(id);

            if (passportToUpdate is null) return NotFound();

            PersonEntity? person = await _personService.GetAsync(passportToUpdate.PersonId);

            if (person is null) return NotFound("Person is null!");

            PictureWorker.RemovePicture(PictureFolders.Passport, passportToUpdate.ScanFileName);

            await _passportService.UpdateAsync(passportToUpdate, passportDTO);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id < 1) return BadRequest();

            PassportEntity? passportToRemove = await _passportService.GetAsync(id);

            if (passportToRemove is null) return NotFound();

            PictureWorker.RemovePicture(PictureFolders.Passport, passportToRemove.ScanFileName);

            await _passportService.RemoveAsync(passportToRemove);

            return NoContent();
        }
    }
}
