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

        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewPassportList])]
        public async Task<ActionResult<IEnumerable<PassportEntity>>> GetListAsync()
            => Ok(await _passportRepository.GetListAsync());

        [HttpGet("{passportId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewPassport])]
        public async Task<ActionResult<PassportEntity>> GetAsync(int passportId)
        {
            if (passportId < 1) return BadRequest();

            PassportEntity? passportEntity = await _passportRepository.GetAsync(passportEntityId: passportId);

            if (passportEntity is null) return NotFound();

            PersonEntity personEntity = await
                _personRepository.GetAsync(personEntityId: passportEntity.PersonEntityId)
                ?? throw new Exception("Person on Passport is null!");

            Authorizing authorizing = new(_userRepository, HttpContext);

            if (!authorizing.IsAdminAndSecretarRole())
            {
                if (personEntity.StudentEntity is null) return Forbid();

                bool userAccess = await authorizing.RequireOwnerAccess
                (
                    new()
                    {
                        IdList = [personEntity.StudentEntity.Id],
                        Role = Student
                    }
                );

                if (userAccess is false) return Forbid();
            }

            return Ok(passportEntity);
        }

        [HttpGet("get-scan-file/{scanFileName}/{key}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetScanFileAsync(string scanFileName, string key)
        {
            try
            {
                IFormFile picture = await
                    PictureRepository
                    .GetAndDecryptPictureAsync
                        (PictureFolders.Passport, scanFileName, StringHasher.Generate32ByteKey(key));

                return File(picture.OpenReadStream(), picture.ContentType);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditPassport])]
        public async Task<ActionResult<PassportDTO>> CreateAsync([FromForm] PassportDTO passportDTO)
        {
            if (passportDTO.ScanFile is null) return BadRequest();

            PersonEntity? personEntity = await _personRepository.GetAsync(personEntityId: passportDTO.PersonEntityId);

            if (personEntity is null) return NotFound("Person is null!");

            await _passportRepository.AddAsync(_passportRepository.Create(passportDTO));

            return Created("PassportEntity", passportDTO);
        }

        [HttpPut("{passportId:int}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditPassport])]
        public async Task<IActionResult> UpdateAsync(int passportId, [FromForm] PassportDTO passportDTO)
        {
            if (passportId < 1) return BadRequest();

            if (passportDTO.ScanFile is null) return BadRequest();

            PassportEntity? passportEntityToUpdate = await _passportRepository.GetAsync(passportEntityId: passportId);

            if (passportEntityToUpdate is null) return NotFound();

            PersonEntity? personEntity = await _personRepository.GetAsync(personEntityId: passportDTO.PersonEntityId);

            if (personEntity is null) return NotFound("Person is null!");

            PictureRepository.RemovePicture(PictureFolders.Passport, passportEntityToUpdate.ScanFileName);

            await _passportRepository.UpdateAsync(passportEntityToUpdate, passportDTO);

            return NoContent();
        }

        [HttpDelete("{passportId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditPassport])]
        public async Task<IActionResult> DeleteAsync(int passportId)
        {
            if (passportId < 1) return BadRequest();

            PassportEntity? passportEntityToRemove = await _passportRepository.GetAsync(passportEntityId: passportId);

            if (passportEntityToRemove is null) return NotFound();

            PictureRepository.RemovePicture(PictureFolders.Passport, passportEntityToRemove.ScanFileName);

            await _passportRepository.RemoveAsync(passportEntityToRemove);

            return NoContent();
        }
    }
}
