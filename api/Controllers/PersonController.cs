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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewPerson])]
        public async Task<ActionResult<IEnumerable<PersonEntity>>> GetListAsync() => Ok(await _personRepository.GetListAsync());

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewPerson])]
        public async Task<ActionResult<PersonEntity>> GetAsync(int id)
        {
            if (id < 1) return BadRequest();

            PersonEntity? person = await _personRepository.GetAsync(id);

            if (person is null) return NotFound();

            if (person.Student is not null)
            {
                ActionResult actionResultUserAccess = await
                    new PermissionService(_userRepository)
                    .RequireUserAccess
                    (
                        HttpContext,
                        [person.Student.Id],
                        Student
                    );

                if (actionResultUserAccess.GetType() != new OkResult().GetType())
                {
                    actionResultUserAccess = await
                    new PermissionService(_userRepository)
                    .RequireUserAccess
                    (
                        HttpContext,
                        [_groupRepository.GetAsync(person.Student.GroupId).Result!.CuratorId],
                        Curator
                    );

                    if (actionResultUserAccess.GetType() != new OkResult().GetType())
                    {
                        actionResultUserAccess = await
                        new PermissionService(_userRepository)
                        .RequireUserAccess
                        (
                            HttpContext,
                            _groupRepository.GetAsync(person.Student.GroupId).Result!.SubjectList.Select(subject => subject.TeacherId).ToArray(),
                            Teacher
                        );

                        if (actionResultUserAccess.GetType() != new OkResult().GetType()) return actionResultUserAccess;
                    }
                }
            }


            return Ok(person);
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
            if (await _personRepository.GetAsync(personDTO.FirstName, personDTO.MiddleName, personDTO.LastName) is not null)
            {
                ModelState.AddModelError("Custom Error", "PersonEntity already Exists!");

                return BadRequest(ModelState);
            }

            PersonEntity personToAdd = _personRepository.Create(personDTO);

            await _personRepository.AddAsync(personToAdd);

            return Created("PersonEntity", personToAdd);
        }

        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditPerson])]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] PersonDTO personDTO)
        {
            if (id < 1) return BadRequest();

            PersonEntity? personToUpdate = await _personRepository.GetAsync(id);

            if (personToUpdate is null) return NotFound();

            if (await _personRepository.GetAsync(personDTO.FirstName, personDTO.MiddleName, personDTO.LastName) is not null)
            {
                ModelState.AddModelError("Custom Error", "PersonEntity already Exists!");

                return BadRequest(ModelState);
            }

            PictureRepository.RemovePicture(PictureFolders.Person, personToUpdate.AvatarFileName);

            await _personRepository.UpdateAsync(personToUpdate, personDTO);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditPerson])]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id < 1) return BadRequest();

            PersonEntity? personToRemove = await _personRepository.GetAsync(id);

            if (personToRemove is null) return NotFound();

            if (personToRemove.Passport is not null)
            {
                PictureRepository.RemovePicture(PictureFolders.Passport, personToRemove.Passport.ScanFileName);
            }

            PictureRepository.RemovePicture(PictureFolders.Person, personToRemove.AvatarFileName);

            await _personRepository.RemoveAsync(personToRemove);

            return NoContent();
        }
    }
}