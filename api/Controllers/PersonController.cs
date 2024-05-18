using api.Data;
using api.Helpers.Constants;
using api.Models;
using api.Model.DTO;
using api.Repositories;
using api.Repositories.Data;
using Microsoft.AspNetCore.Mvc;
using api.Extensions;

using static api.Helpers.Enums.RoleEnum;
using static api.Helpers.Enums.PermissionEnum;

namespace api.Controllers
{
    [Route("person")]
    [ApiController]
    [RequireRoles([Admin, Secretar, Curator, Student])]
    public class PersonController(AppDbContext db) : ControllerBase
    {
        private readonly PersonRepository _personService = new(db);

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([Read])]
        public async Task<ActionResult<IEnumerable<PersonEntity>>> GetListAsync() => Ok(await _personService.GetListAsync());

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([Read])]
        public async Task<ActionResult<PersonEntity>> GetAsync(int id)
        {
            if (id < 1) return BadRequest();

            PersonEntity? person = await _personService.GetAsync(id);

            if (person is null) return NotFound();

            return Ok(person);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([Create])]
        public async Task<ActionResult<PersonEntity>> CreateAsync([FromForm] PersonDTO personDTO)
        {
            if (await _personService.GetAsync(personDTO.FirstName, personDTO.MiddleName, personDTO.LastName) is not null)
            {
                ModelState.AddModelError("Custom Error", "PersonEntity already Exists!");

                return BadRequest(ModelState);
            }

            PersonEntity personToAdd = new()
            {
                FirstName = personDTO.FirstName,
                MiddleName = personDTO.MiddleName,
                LastName = personDTO.LastName,
                BirthDate = personDTO.BirthDate,
                Sex = personDTO.Sex,
                AvatarFileName = await PictureRepository.UploadPersonAvatarAsync(personDTO.Avatar, personDTO.Sex),
            };

            await _personService.AddAsync(personToAdd);

            return Created("PersonEntity", personToAdd);
        }

        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([Update])]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] PersonDTO personDTO)
        {
            if (id < 1) return BadRequest();

            PersonEntity? personToUpdate = await _personService.GetAsync(id);

            if (personToUpdate is null) return NotFound();

            if (await _personService.GetAsync(personDTO.FirstName, personDTO.MiddleName, personDTO.LastName) is not null)
            {
                ModelState.AddModelError("Custom Error", "PersonEntity already Exists!");

                return BadRequest(ModelState);
            }

            PictureRepository.RemovePicture(PictureFolders.Person, personToUpdate.AvatarFileName);

            await _personService.UpdateAsync(personToUpdate, personDTO);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([Delete])]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id < 1) return BadRequest();

            PersonEntity? personToRemove = await _personService.GetAsync(id);

            if (personToRemove is null) return NotFound();

            if (personToRemove.Passport is not null)
            {
                PictureRepository.RemovePicture(PictureFolders.Passport, personToRemove.Passport.ScanFileName);
            }

            PictureRepository.RemovePicture(PictureFolders.Person, personToRemove.AvatarFileName);

            await _personService.RemoveAsync(personToRemove);

            return NoContent();
        }
    }
}