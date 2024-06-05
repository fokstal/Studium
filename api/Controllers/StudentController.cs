using api.Data;
using api.Models;
using api.Model.DTO;
using api.Repositories.Data;
using Microsoft.AspNetCore.Mvc;
using api.Extensions;
using api.Services;
using api.Models.DTO;

using static api.Helpers.Enums.RoleEnum;
using static api.Helpers.Enums.PermissionEnum;

namespace api.Controllers
{
    [Route("student")]
    [ApiController]
    [RequireRoles([Admin, Secretar, Curator, Student])]
    public class StudentController(AppDbContext db) : ControllerBase
    {
        private readonly StudentRepository _studentRepository = new(db);
        private readonly PersonRepository _personRepository = new(db);
        private readonly GroupRepository _groupRepository = new(db);
        private readonly UserRepository _userRepository = new(db);

        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewStudentList])]
        public async Task<ActionResult<IEnumerable<StudentEntity>>> GetListAsync()
            => Ok(await _studentRepository.GetListAsync());

        [HttpGet("{studentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewStudent])]
        public async Task<ActionResult<StudentEntity>> GetAsync(Guid studentId)
        {
            StudentEntity? studentEntity = await _studentRepository.GetAsync(studentEntityId: studentId);

            if (studentEntity is null) return NotFound();

            Authorizing authorizing = new(_userRepository, HttpContext);

            if (!authorizing.IsAdminAndSecretarRole())
            {
                bool userAccess = await authorizing.RequireOwnerListAccess
                    ([
                        new()
                        {
                            IdList = [_groupRepository.GetAsync(studentEntity.GroupEntityId).Result!.CuratorId],
                            Role = Curator
                        },
                        new()
                        {
                            IdList = [studentEntity.Id],
                            Role = Student
                        },
                        new()
                        {
                            IdList =
                                _groupRepository
                                .GetAsync(studentEntity.GroupEntityId)
                                .Result!
                                .SubjectEntityList.Select(s => s.TeacherId)
                                .ToArray(),

                            Role = Teacher
                        },
                    ]);

                if (userAccess is false) return Forbid();

            }

            return Ok(studentEntity);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditStudent])]
        public async Task<ActionResult<StudentDTO>> CreateAsync([FromBody] StudentDTO studentDTO)
        {
            if (await _studentRepository.GetAsync
                (
                    personEntityId: studentDTO.PersonEntityId,
                    groupEntityId: studentDTO.GroupEntityId
                ) is not null)
            {
                ModelState.AddModelError("Custom Error", "StudentEntity already Exists!");

                return BadRequest(ModelState);
            }

            PersonEntity? personEntity = await _personRepository.GetAsync(personEntityId: studentDTO.PersonEntityId);

            if (personEntity is null) return NotFound("Person is null!");

            if (studentDTO.GroupEntityId is not null)
            {
                GroupEntity? groupEntity = await _groupRepository.GetAsync(groupEntityId: studentDTO.GroupEntityId);

                if (groupEntity is null) return NotFound("Group is null!");
            }

            Guid userStudentId = Guid.NewGuid();
            string login = Guid.NewGuid().ToString()[..10];

            await _userRepository.AddAsync(_userRepository.Create(new RegisterUserDTO()
            {
                Id = userStudentId,
                Login = login,
                FirstName = personEntity.FirstName,
                MiddleName = personEntity.MiddleName,
                LastName = personEntity.LastName,
                Password = login,
                RoleEnumList = [Student]
            }));

            studentDTO.Id = userStudentId;

            await _studentRepository.AddAsync(_studentRepository.Create(studentDTO));

            return Created("StudentEntity", studentDTO);
        }

        [HttpPut("{studentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditStudent])]
        public async Task<IActionResult> UpdateAsync(Guid studentId, [FromBody] StudentDTO studentDTO)
        {
            StudentEntity? studentEntityToUpdate = await _studentRepository.GetAsync(studentEntityId: studentId);

            if (studentEntityToUpdate is null) return NotFound();

            if (await _studentRepository.GetAsync
                (
                    personEntityId: studentDTO.PersonEntityId,
                    groupEntityId: studentDTO.GroupEntityId
                ) is not null)
            {
                ModelState.AddModelError("Custom Error", "StudentEntity already Exists!");

                return BadRequest(ModelState);
            }

            PersonEntity? person = await _personRepository.GetAsync(personEntityId: studentDTO.PersonEntityId);

            if (person is null) return NotFound("Person is null!");

            int? groupId = null;

            if (studentDTO.GroupEntityId is not null)
            {
                GroupEntity? group = await _groupRepository.GetAsync(groupEntityId: studentDTO.GroupEntityId);

                if (group is null) return NotFound("Group is null!");

                groupId = group.Id;
            }

            studentDTO.GroupEntityId = groupId;

            UserEntity userEntityToUpdate = await _userRepository.GetNoTrackingRoleAsync(userEntityId: studentEntityToUpdate.Id) ?? throw new Exception("User on Student is null!");

            await _userRepository.UpdateAsync(userEntityToUpdate, new()
            {
                Login = userEntityToUpdate.Login,
                FirstName = person.FirstName,
                MiddleName = person.MiddleName,
                LastName = person.LastName,
                Password = userEntityToUpdate.Login,
                RoleEnumList = [Student]
            });

            await _studentRepository.UpdateAsync(studentEntityToUpdate, studentDTO);

            return NoContent();
        }

        [HttpPut("date/{studentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditStudent])]
        public async Task<IActionResult> UpdateAsync(Guid studentId, [FromBody] DateStudentDTO dateStudentDTO)
        {
            StudentEntity? studentEntityToUpdate = await _studentRepository.GetAsync(studentEntityId: studentId);

            if (studentEntityToUpdate is null) return NotFound();

            return NoContent();
        }

        [HttpDelete("{studentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditStudent])]
        public async Task<IActionResult> DeleteAsync(Guid studentId)
        {
            StudentEntity? studentEntityToRemove = await _studentRepository.GetAsync(studentEntityId: studentId);

            if (studentEntityToRemove is null) return NotFound();

            UserEntity userEntityToRemove = await _userRepository.GetNoTrackingAsync(userEntityId: studentEntityToRemove.Id) ?? throw new Exception("User on Student is null!");

            await _userRepository.RemoveAsync(userEntityToRemove);

            await _studentRepository.RemoveAsync(studentEntityToRemove);

            return NoContent();
        }
    }
}