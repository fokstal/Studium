using api.Data;
using api.Model;
using api.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("person")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetListAsync()
        {
            using (AppDbContext db = new())
            {
                IEnumerable<Person> personList = await db.Person.Include(personDb => personDb.Passport).ToArrayAsync();

                return Ok(personList);
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
                Person? person = await db.Person.FirstOrDefaultAsync(personDb => personDb.Id == id);

                if (person is null) return NotFound();

                return Ok(person);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateAsync([FromBody] PersonDTO personDTO)
        {
            using (AppDbContext db = new())
            {
                if (await db.Person.FirstOrDefaultAsync
                (
                    personDb =>
                        personDb.FirstName.ToLower() == personDTO.FirstName.ToLower() &&
                        personDb.MiddleName.ToLower() == personDTO.MiddleName.ToLower() &&
                        personDb.LastName.ToLower() == personDTO.LastName.ToLower()
                ) is not null)
                {
                    ModelState.AddModelError("Custom Error", "Person already Exists!");

                    return BadRequest(ModelState);
                }

                Passport? passport = await db.Passport.FirstOrDefaultAsync(passportDb => passportDb.Id == personDTO.PassportId);

                if (passport is null) return NotFound("Passport is null!");

                await db.Person.AddAsync(new()
                {
                    FirstName = personDTO.FirstName,
                    MiddleName = personDTO.MiddleName,
                    LastName = personDTO.LastName,
                    BirthDate = personDTO.BirthDate,
                    PassportId = passport.Id,
                    Passport = passport,
                });

                await db.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAsync), new { id = personDTO.Id }, personDTO);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] PersonDTO personDTO)
        {
            if (id < 1) return BadRequest();

            using (AppDbContext db = new())
            {
                Person? personToUpdate = await db.Person.FirstOrDefaultAsync(personDb => personDb.Id == id);

                if (personToUpdate is null) return NotFound();

                if (await db.Person.FirstOrDefaultAsync
                (
                    personDb =>
                        personDb.FirstName.ToLower() == personDTO.FirstName.ToLower() &&
                        personDb.MiddleName.ToLower() == personDTO.MiddleName.ToLower() &&
                        personDb.LastName.ToLower() == personDTO.LastName.ToLower()
                ) is not null)
                {
                    ModelState.AddModelError("Custom Error", "Person already Exists!");

                    return BadRequest(ModelState);
                }

                Passport? passport = await db.Passport.FirstOrDefaultAsync(passportDb => passportDb.Id == personDTO.PassportId);

                if (passport is null) return NotFound("Passport is null!");

                personToUpdate.FirstName = personDTO.FirstName;
                personToUpdate.MiddleName = personDTO.MiddleName;
                personToUpdate.LastName = personDTO.LastName;
                personToUpdate.BirthDate = personDTO.BirthDate;
                personToUpdate.PassportId = passport.Id;
                personToUpdate.Passport = passport;

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
                Person? person = await db.Person.FirstOrDefaultAsync(personDb => personDb.Id == id);

                if (person is null) return NotFound();

                db.Person.Remove(person);

                await db.SaveChangesAsync();

                return NoContent();
            }
        }
    }
}