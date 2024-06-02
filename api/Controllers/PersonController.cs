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
using api.Services;

namespace api.Controllers
{
    [Route("person")]
    [ApiController]
    [RequireRoles([Admin, Secretar, Curator, Student])]
    public class PersonController(AppDbContext db) : ControllerBase
    {
        private readonly PersonRepository _personRepository = new(db);
        private readonly GroupRepository _groupRepository = new(db);
        private readonly UserRepository _userRepository = new(db);

        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewPersonList])]
        public async Task<ActionResult<IEnumerable<PersonEntity>>> GetListAsync()
            => Ok(await _personRepository.GetListAsync());

        [HttpGet("{personId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewPerson])]
        public async Task<ActionResult<PersonEntity>> GetAsync(int personId)
        {
            if (personId < 1) return BadRequest();

            PersonEntity? personEntity = await _personRepository.GetAsync(personEntityId: personId);

            if (personEntity is null) return NotFound();

            // if (person.Student is not null)
            // {
            //     bool userAccess = await new Authorizing(_userRepository, HttpContext).RequireOwnerListAccess
            //     ([
            //         new()
            //         {
            //             IdList = [person.Student.Id],
            //             Role = Student
            //         },
            //         new()
            //         {
            //             IdList = [_groupRepository.GetAsync(person.Student.GroupId).Result!.CuratorId],
            //             Role = Curator
            //         },
            //         new()
            //         {
            //             IdList = _groupRepository.GetAsync(person.Student.GroupId).Result!.SubjectList.Select(subject => subject.TeacherId).ToArray(),
            //             Role = Teacher
            //         },
            //     ]);

            //     if (userAccess is false) return Forbid();
            // }


            return Ok(personEntity);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditPerson])]
        public async Task<ActionResult<PersonEntity>> CreateAsync([FromForm] PersonDTO personDTO)
        {
            if (await _personRepository.GetAsync
                (
                    firstName: personDTO.FirstName,
                    middleName: personDTO.MiddleName,
                    lastName: personDTO.LastName
                ) is not null)
            {
                ModelState.AddModelError("Custom Error", "PersonEntity already Exists!");

                return BadRequest(ModelState);
            }

            PersonEntity personEntityToAdd = _personRepository.Create(personDTO);

            await _personRepository.AddAsync(personEntityToAdd);

            return Created("PersonEntity", personEntityToAdd);
        }

        [HttpPut("{personId:int}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditPerson])]
        public async Task<IActionResult> UpdateAsync(int personId, [FromForm] PersonDTO personDTO)
        {
            if (personId < 1) return BadRequest();

            PersonEntity? personEntityToUpdate = await _personRepository.GetAsync(personEntityId: personId);

            if (personEntityToUpdate is null) return NotFound();

            if (await _personRepository.GetAsync
                (
                    firstName: personDTO.FirstName,
                    middleName: personDTO.MiddleName,
                    lastName: personDTO.LastName
                ) is not null)
            {
                ModelState.AddModelError("Custom Error", "PersonEntity already Exists!");

                return BadRequest(ModelState);
            }

            PictureRepository.RemovePicture(PictureFolders.Person, personEntityToUpdate.AvatarFileName);

            await _personRepository.UpdateAsync(personEntityToUpdate, personDTO);

            return NoContent();
        }

        [HttpDelete("{personId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditPerson])]
        public async Task<IActionResult> DeleteAsync(int personId)
        {
            if (personId < 1) return BadRequest();

            PersonEntity? personEntityToRemove = await _personRepository.GetAsync(personEntityId: personId);

            if (personEntityToRemove is null) return NotFound();

            if (personEntityToRemove.PassportEntity is not null)
            {
                PictureRepository.RemovePicture(PictureFolders.Passport, personEntityToRemove.PassportEntity.ScanFileName);
            }

            PictureRepository.RemovePicture(PictureFolders.Person, personEntityToRemove.AvatarFileName);

            await _personRepository.RemoveAsync(personEntityToRemove);

            return NoContent();
        }
    }
}