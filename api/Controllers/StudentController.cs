using api.Data;
using api.Models;
using api.Model.DTO;
using api.Repositories.Data;
using Microsoft.AspNetCore.Mvc;
using api.Extensions;

using static api.Helpers.Enums.RoleEnum;
using static api.Helpers.Enums.PermissionEnum;
using api.Services;

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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewStudent])]
        public async Task<ActionResult<IEnumerable<StudentEntity>>> GetListAsync() => Ok(await _studentRepository.GetListAsync());

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewStudentList])]
        public async Task<ActionResult<StudentEntity>> GetAsync(Guid id)
        {
            StudentEntity? student = await _studentRepository.GetAsync(id);

            if (student is null) return NotFound();

            // bool userAccess = await new Authorizing(_userRepository, HttpContext).RequireOwnerListAccess
            //     ([
            //         new()
            //         {
            //             IdList = [_groupRepository.GetAsync(student.GroupId).Result!.CuratorId],
            //             Role = Curator
            //         },
            //         new()
            //         {
            //             IdList = [student.Id],
            //             Role = Student
            //         },
            //         new()
            //         {
            //             IdList = _groupRepository.GetAsync(student.GroupId).Result!.SubjectList.Select(subject => subject.TeacherId).ToArray(),
            //             Role = Teacher
            //         },
            //     ]);

            // if (userAccess is false) return Forbid();

            return Ok(student);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditStudent])]
        public async Task<ActionResult<StudentDTO>> CreateAsync([FromBody] StudentDTO studentDTO)
        {
            if (await _studentRepository.GetAsync(studentDTO.PersonId, studentDTO.GroupId) is not null)
            {
                ModelState.AddModelError("Custom Error", "StudentEntity already Exists!");

                return BadRequest(ModelState);
            }

            PersonEntity? person = await _personRepository.GetAsync(studentDTO.PersonId);

            if (person is null) return NotFound("Person is null!");

            if (studentDTO.GroupId is not null)
            {
                GroupEntity? group = await _groupRepository.GetAsync(studentDTO.GroupId);

                if (group is null) return NotFound("Group is null!");
            }

            Guid userStudentId = Guid.NewGuid();
            string login = Guid.NewGuid().ToString()[..10];

            await _userRepository.AddAsync(_userRepository.Create(new RegisterUserDTO()
            {
                Id = userStudentId,
                Login = login,
                FirstName = person.FirstName,
                MiddleName = person.MiddleName,
                LastName = person.LastName,
                Password = login,
                RoleList = [Student]
            }));

            studentDTO.Id = userStudentId;

            await _studentRepository.AddAsync(_studentRepository.Create(studentDTO));

            return Created("StudentEntity", studentDTO);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditStudent])]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] StudentDTO studentDTO)
        {
            if (await _studentRepository.GetAsync(studentDTO.PersonId, studentDTO.GroupId) is not null)
            {
                ModelState.AddModelError("Custom Error", "StudentEntity already Exists!");

                return BadRequest(ModelState);
            }

            StudentEntity? studentToUpdate = await _studentRepository.GetAsync(id);

            if (studentToUpdate is null) return NotFound();

            PersonEntity? person = await _personRepository.GetAsync(studentDTO.PersonId);

            if (person is null) return NotFound("Person is null!");

            int? groupId = null;

            if (studentDTO.GroupId is not null)
            {
                GroupEntity? group = await _groupRepository.GetAsync(studentDTO.GroupId);

                if (group is null) return NotFound("Group is null!");

                groupId = group.Id;
            }

            studentDTO.GroupId = groupId;

            UserEntity userToRemove = await _userRepository.GetAsync(studentToUpdate.Id) ?? throw new Exception("User on Student is null!");

            await _userRepository.RemoveAsync(userToRemove);

            Guid userStudentId = Guid.NewGuid();
            string login = Guid.NewGuid().ToString()[..10];

            await _userRepository.AddAsync(_userRepository.Create(new RegisterUserDTO()
            {
                Id = userStudentId,
                Login = login,
                FirstName = person.FirstName,
                MiddleName = person.MiddleName,
                LastName = person.LastName,
                Password = login,
                RoleList = [Student]
            }));

            studentDTO.Id = userStudentId;

            await _studentRepository.UpdateAsync(studentToUpdate, studentDTO);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditStudent])]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            StudentEntity? studentToRemove = await _studentRepository.GetAsync(id);

            if (studentToRemove is null) return NotFound();

            UserEntity userToRemove = await _userRepository.GetAsync(studentToRemove.Id) ?? throw new Exception("User on Student is null!");

            await _userRepository.RemoveAsync(userToRemove);

            await _studentRepository.RemoveAsync(studentToRemove);

            return NoContent();
        }
    }
}