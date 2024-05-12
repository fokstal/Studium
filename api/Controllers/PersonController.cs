using api.Data;
using api.Model;
using api.Model.DTO;
using api.Services.DataServices;
using Microsoft.AspNetCore.Mvc;

using static api.Service.PictureService;

namespace api.Controllers
{
    [Route("person")]
    [ApiController]
    public class PersonController(AppDbContext db) : ControllerBase
    {
        private readonly PersonService _personService = new(db);

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Person>>> GetListAsync() => Ok(await _personService.GetListAsync());

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Person>> GetAsync(int id)
        {
            if (id < 1) return BadRequest();

            Person? person = await _personService.GetAsync(id);

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
            if (await _personService.GetAsync(personDTO.FirstName, personDTO.MiddleName, personDTO.LastName) is not null)
            {
                ModelState.AddModelError("Custom Error", "Person already Exists!");

                return BadRequest(ModelState);
            }

            await _personService.AddAsync(new()
            {
                FirstName = personDTO.FirstName,
                MiddleName = personDTO.MiddleName,
                LastName = personDTO.LastName,
                BirthDate = personDTO.BirthDate,
                Sex = personDTO.Sex,
                AvatarFileName = await UploadPersonAvatarAsync(personDTO.Avatar, personDTO.Sex),
            });

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

            Person? personToUpdate = await _personService.GetAsync(id);

            if (personToUpdate is null) return NotFound();

            if (await _personService.GetAsync(personDTO.FirstName, personDTO.MiddleName, personDTO.LastName) is not null)
            {
                ModelState.AddModelError("Custom Error", "Person already Exists!");

                return BadRequest(ModelState);
            }

            System.IO.File.Delete($"{picturesFolderPath}/Person/{personToUpdate.AvatarFileName}");

            await _personService.UpdateAsync(personToUpdate, personDTO);

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

            Person? personToRemove = await _personService.GetAsync(id);

            if (personToRemove is null) return NotFound();

            if (personToRemove.Passport is not null)
            {
                System.IO.File.Delete($"{picturesFolderPath}/Passport/{personToRemove.Passport.ScanFileName}");
            }

            System.IO.File.Delete($"{picturesFolderPath}/Person/{personToRemove.AvatarFileName}");

            await _personService.RemoveAsync(personToRemove);

            return NoContent();
        }
    }
}