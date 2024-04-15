using api.Data;
using api.Model;
using api.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("passport")]
    [ApiController]
    public class PassportController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetListAsync()
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
        public async Task<ActionResult> GetAsync(int id)
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateAsync([FromBody] PassportDTO passportDTO)
        {
            using (AppDbContext db = new())
            {
                Person? person = await db.Person.FirstOrDefaultAsync(personDb => personDb.Id == passportDTO.PersonId);

                if (person is null) return NotFound("Person is null!");

                await db.Passport.AddAsync(new()
                {
                    Photo = passportDTO.Photo,
                    PersonId = person.Id,
                });

                await db.SaveChangesAsync();

                return Created("Passport", passportDTO);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] PassportDTO passportDTO)
        {
            if (id < 1) return BadRequest();

            using (AppDbContext db = new())
            {
                Passport? passportToUpdate = await db.Passport.FirstOrDefaultAsync(passportDb => passportDb.Id == id);

                if (passportToUpdate is null) return NotFound();

                Person? person = await db.Person.FirstOrDefaultAsync(personDb => personDb.Id == passportDTO.PersonId);

                if (person is null) return NotFound("Person is null!");

                passportToUpdate.Photo = passportDTO.Photo;
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

                db.Passport.Remove(passport);

                await db.SaveChangesAsync();

                return NoContent();
            }
        }
    }
}