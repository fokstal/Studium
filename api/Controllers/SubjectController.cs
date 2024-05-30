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
    [Route("subject")]
    [ApiController]
    [RequireRoles([Admin, Secretar, Curator, Teacher, Student])]
    public class SubjectController(AppDbContext db) : ControllerBase
    {
        private readonly SubjectRepository _subjectRepository = new(db);
        private readonly GroupRepository _groupRepository = new(db);
        private readonly UserRepository _userRepository = new(db);

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewSubject])]
        public async Task<ActionResult<IEnumerable<SubjectEntity>>> GetListAsync() => Ok(await _subjectRepository.GetListAsync());

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewSubject])]
        public async Task<ActionResult<SubjectEntity>> GetAsync(int id)
        {
            if (id < 1) return BadRequest();

            SubjectEntity? subject = await _subjectRepository.GetAsync(id);

            if (subject is null) return NotFound();

            // bool userAccess = await new Authorizing(_userRepository, HttpContext).RequireOwnerListAccess
            //     ([
            //         new()
            //         {
            //             IdList = [subject.TeacherId],
            //             Role = Teacher
            //         },
            //         new()
            //         {
            //             IdList = [_groupRepository.GetAsync(subject).Result.CuratorId],
            //             Role = Curator
            //         },
            //         new()
            //         {
            //             IdList = _groupRepository.GetAsync(subject).Result.StudentList.Select(student => student.Id).ToArray(),
            //             Role = Student
            //         },
            //     ]);

            // if (userAccess is false) return Forbid();

            return Ok(subject);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditSubject])]
        public async Task<ActionResult<SubjectDTO>> CreateAsync([FromBody] SubjectDTO subjectDTO)
        {
            if (await _subjectRepository.GetAsync(subjectDTO.Name, subjectDTO.TeacherId) is not null)
            {
                ModelState.AddModelError("Custom Error", "SubjectEntity already Exists!");

                return BadRequest(ModelState);
            }

            if (subjectDTO.GroupId is not null)
            {
                GroupEntity? group = await _groupRepository.GetAsync(subjectDTO.GroupId);

                if (group is null) return NotFound("Group is null!");
            }

            UserEntity? user = await _userRepository.GetNoTrackingAsync(subjectDTO.TeacherId);

            if (user is null) return NotFound("Teacher is null");
            if (!UserService.CheckRoleContains(_userRepository, user, Teacher)) return BadRequest("User is not a Teacher!");

            await _subjectRepository.AddAsync(_subjectRepository.Create(subjectDTO));

            return Created("SubjectEntity", subjectDTO);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditSubject])]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] SubjectDTO subjectDTO)
        {
            if (id < 1) return BadRequest();

            SubjectEntity? subjectToUpdate = await _subjectRepository.GetAsync(id);
            SubjectEntity? subjectAnother = await _subjectRepository.GetAsync(subjectDTO.Name, subjectDTO.TeacherId);

            if (subjectToUpdate is null) return NotFound();

            if (subjectAnother is not null && subjectAnother.Id != subjectToUpdate.Id)
            {
                ModelState.AddModelError("Custom Error", "SubjectEntity already Exists!");

                return BadRequest(ModelState);
            }

            int? groupId = null;

            if (subjectDTO.GroupId is not null)
            {
                GroupEntity? group = await _groupRepository.GetAsync(subjectDTO.GroupId);

                if (group is null) return NotFound("Group is null!");

                groupId = group.Id;
            }

            subjectDTO.GroupId = groupId;

            UserEntity? user = await _userRepository.GetNoTrackingAsync(subjectDTO.TeacherId);

            if (user is null) return NotFound("Teacher is null");
            if (!UserService.CheckRoleContains(_userRepository, user, Teacher)) return BadRequest("User is not a Teacher!");

            await _subjectRepository.UpdateAsync(subjectToUpdate, subjectDTO);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditSubject])]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id < 1) return BadRequest();

            SubjectEntity? subjectToRemove = await _subjectRepository.GetAsync(id);

            if (subjectToRemove is null) return NotFound();

            await _subjectRepository.RemoveAsync(subjectToRemove);

            return NoContent();
        }
    }
}