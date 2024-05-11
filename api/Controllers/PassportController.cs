using api.Data;
using api.Model;
using api.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using static api.Service.PictureService;

namespace api.Controllers
{
    [Route("passport")]
    [ApiController]
    public class PassportController(AppDbContext db) : ControllerBase
    {
        private readonly AppDbContext _db = db;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Passport>>> GetListAsync()
        {
            IEnumerable<Passport> passportList = await _db.Passport.ToArrayAsync();

            return Ok(passportList);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Passport>> GetAsync(int id)
        {
            if (id < 1) return BadRequest();

            Passport? passport = await _db.Passport.FirstOrDefaultAsync(passportDb => passportDb.Id == id);

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

            Person? person = await _db.Person.FirstOrDefaultAsync(personDb => personDb.Id == passportDTO.PersonId);

            if (person is null) return NotFound("Person is null!");

            await _db.Passport.AddAsync(new()
            {
                ScanFileName = await UploadPassportScanAsync(passportDTO.Scan),
                PersonId = person.Id,
            });

            await _db.SaveChangesAsync();

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

            Passport? passportToUpdate = await _db.Passport.FirstOrDefaultAsync(passportDb => passportDb.Id == id);

            if (passportToUpdate is null) return NotFound();

            Person? person = await _db.Person.FirstOrDefaultAsync(personDb => personDb.Id == passportDTO.PersonId);

            if (person is null) return NotFound("Person is null!");

            System.IO.File.Delete($"{picturesFolderPath}/Passport/{passportToUpdate.ScanFileName}");

            passportToUpdate.ScanFileName = await UploadPassportScanAsync(passportDTO.Scan);
            passportToUpdate.PersonId = person.Id;

            await _db.SaveChangesAsync();

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

            Passport? passport = await _db.Passport.FirstOrDefaultAsync(passportDb => passportDb.Id == id);

            if (passport is null) return NotFound();

            System.IO.File.Delete($"{picturesFolderPath}/Passport/{passport.ScanFileName}");

            _db.Passport.Remove(passport);

            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
