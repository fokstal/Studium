using api.Data;
using api.Model;
using api.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using static api.Service.PictureService;

namespace api.Controllers
{
    [Route("person")]
    [ApiController]
    public class PersonController(AppDbContext db) : ControllerBase
    {
        private readonly AppDbContext _db = db;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Person>>> GetListAsync()
        {
            IEnumerable<Person> personList = await _db.Person.Include(personDb => personDb.Passport).Include(personDb => personDb.Student).ToArrayAsync();

            return Ok(personList);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Person>> GetAsync(int id)
        {
            if (id < 1) return BadRequest();

            Person? person = await _db.Person.Include(personDb => personDb.Passport).Include(personDb => personDb.Student).FirstOrDefaultAsync(personDb => personDb.Id == id);

            if (person is null) return NotFound();

            return Ok(person);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Person>> CreateAsync([FromForm] PersonDTO personDTO)
        {
            if (await _db.Person.FirstOrDefaultAsync
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

            await _db.Person.AddAsync(new()
            {
                FirstName = personDTO.FirstName,
                MiddleName = personDTO.MiddleName,
                LastName = personDTO.LastName,
                BirthDate = personDTO.BirthDate,
                Sex = personDTO.Sex,
                AvatarFileName = await UploadPersonAvatarAsync(personDTO.Avatar, personDTO.Sex),
            });

            await _db.SaveChangesAsync();

            return Created("Person", personDTO);
        }

        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateAsync(int id, [FromForm] PersonDTO personDTO)
        {
            if (id < 1) return BadRequest();

            Person? personToUpdate = await _db.Person.FirstOrDefaultAsync(personDb => personDb.Id == id);

            if (personToUpdate is null) return NotFound();

            if (await _db.Person.FirstOrDefaultAsync
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

            System.IO.File.Delete($"{picturesFolderPath}/Person/{personToUpdate.AvatarFileName}");

            personToUpdate.FirstName = personDTO.FirstName;
            personToUpdate.MiddleName = personDTO.MiddleName;
            personToUpdate.LastName = personDTO.LastName;
            personToUpdate.BirthDate = personDTO.BirthDate;
            personToUpdate.Sex = personDTO.Sex;
            personToUpdate.AvatarFileName = await UploadPersonAvatarAsync(personDTO.Avatar, personToUpdate.Sex);

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

            Person? person = await _db.Person.Include(personDb => personDb.Passport).FirstOrDefaultAsync(personDb => personDb.Id == id);

            if (person is null) return NotFound();

            if (person.Passport is not null)
            {
                System.IO.File.Delete($"{picturesFolderPath}/Passport/{person.Passport.ScanFileName}");
            }

            System.IO.File.Delete($"{picturesFolderPath}/Person/{person.AvatarFileName}");

            _db.Person.Remove(person);

            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}