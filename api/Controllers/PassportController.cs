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
    public class PassportController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Passport>>> GetListAsync()
        {
            using (AppDbContext db = new())
            {
                IEnumerable<Passport> passportList = await db.Passport.ToArrayAsync();

                return Ok(passportList);
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Passport>> GetAsync(int id)
        {
            if (id < 1) return BadRequest();

            using (AppDbContext db = new())
            {
                Passport? passport = await db.Passport.FirstOrDefaultAsync(passportDb => passportDb.Id == id);

                if (passport is null) return NotFound();

                return Ok(passport);
            }
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Passport>> CreateAsync([FromForm] PassportDTO passportDTO)
        {
            if (passportDTO.Scan is null) return BadRequest();

            using (AppDbContext db = new())
            {
                Person? person = await db.Person.FirstOrDefaultAsync(personDb => personDb.Id == passportDTO.PersonId);

                if (person is null) return NotFound("Person is null!");

                await db.Passport.AddAsync(new()
                {
                    ScanFileName = await UploadPassportScanAsync(passportDTO.Scan),
                    PersonId = person.Id,
                });

                await db.SaveChangesAsync();

                return Created("Passport", passportDTO);
            }
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

            using (AppDbContext db = new())
            {
                Passport? passportToUpdate = await db.Passport.FirstOrDefaultAsync(passportDb => passportDb.Id == id);

                if (passportToUpdate is null) return NotFound();

                Person? person = await db.Person.FirstOrDefaultAsync(personDb => personDb.Id == passportDTO.PersonId);

                if (person is null) return NotFound("Person is null!");

                System.IO.File.Delete($"{picturesFolderPath}/Passport/{passportToUpdate.ScanFileName}");

                passportToUpdate.ScanFileName = await UploadPassportScanAsync(passportDTO.Scan);
                passportToUpdate.PersonId = person.Id;

                await db.SaveChangesAsync();

                return NoContent();
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            if (id < 1) return BadRequest();

            using (AppDbContext db = new())
            {
                Passport? passport = await db.Passport.FirstOrDefaultAsync(passportDb => passportDb.Id == id);

                if (passport is null) return NotFound();

                System.IO.File.Delete($"{picturesFolderPath}/Passport/{passport.ScanFileName}");

                db.Passport.Remove(passport);

                await db.SaveChangesAsync();

                return NoContent();
            }
        }
    }
}