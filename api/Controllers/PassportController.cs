using api.Data;
using api.Models;
using api.Model.DTO;
using api.Repositories;
using api.Repositories.Data;
using api.Helpers.Constants;
using Microsoft.AspNetCore.Mvc;
using api.Extensions;

using static api.Helpers.Enums.RoleEnum;
using static api.Helpers.Enums.PermissionEnum;
using api.Services;

namespace api.Controllers
{
    [Route("passport")]
    [ApiController]
    [RequireRoles([Admin, Secretar, Student])]
    public class PassportController(AppDbContext db) : ControllerBase
    {
        private readonly PassportRepository _passportRepository = new(db);
        private readonly PersonRepository _personRepository = new(db);
        private readonly UserRepository _userRepository = new(db);

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewPassportList])]
        public async Task<ActionResult<IEnumerable<PassportEntity>>> GetListAsync() => Ok(await _passportRepository.GetListAsync());

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewPassport])]
        public async Task<ActionResult<PassportEntity>> GetAsync(int id)
        {
            if (id < 1) return BadRequest();

            PassportEntity? passport = await _passportRepository.GetAsync(id);

            if (passport is null) return NotFound();

            PersonEntity? person = await _personRepository.GetAsync(passport.PersonId);

            if (person!.Student is not null)
            {
                bool userAccess = await new Authorizing(_userRepository, HttpContext).RequireOwnerAccess
                (
                    new()
                    {
                        IdList = [person.Student.Id],
                        Role = Student
                    }
                );

                if (userAccess is false) return Forbid();
            }

            return Ok(passport);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditPassport])]
        public async Task<ActionResult<PassportDTO>> CreateAsync([FromForm] PassportDTO passportDTO)
        {
            if (passportDTO.Scan is null) return BadRequest();

            PersonEntity? person = await _personRepository.GetAsync(passportDTO.PersonId);

            if (person is null) return NotFound("Person is null!");

            await _passportRepository.AddAsync(_passportRepository.Create(passportDTO));

            return Created("PassportEntity", passportDTO);
        }

        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditPassport])]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] PassportDTO passportDTO)
        {
            if (id < 1) return BadRequest();

            if (passportDTO.Scan is null) return BadRequest();

            PassportEntity? passportToUpdate = await _passportRepository.GetAsync(id);

            if (passportToUpdate is null) return NotFound();

            PersonEntity? person = await _personRepository.GetAsync(passportDTO.PersonId);

            if (person is null) return NotFound("Person is null!");

            PictureRepository.RemovePicture(PictureFolders.Passport, passportToUpdate.ScanFileName);

            await _passportRepository.UpdateAsync(passportToUpdate, passportDTO);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditPassport])]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id < 1) return BadRequest();

            PassportEntity? passportToRemove = await _passportRepository.GetAsync(id);

            if (passportToRemove is null) return NotFound();

            PictureRepository.RemovePicture(PictureFolders.Passport, passportToRemove.ScanFileName);

            await _passportRepository.RemoveAsync(passportToRemove);

            return NoContent();
        }
    }
}
