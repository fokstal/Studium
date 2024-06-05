using api.Data;
using api.Models;
using api.Model.DTO;
using api.Repositories.Data;
using Microsoft.AspNetCore.Mvc;
using api.Extensions;
using api.Services;
using api.Helpers.Enums;

using static api.Helpers.Enums.RoleEnum;
using static api.Helpers.Enums.PermissionEnum;

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

        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewSubjectList])]
        public async Task<ActionResult<IEnumerable<SubjectEntity>>> GetListAsync()
            => Ok(await _subjectRepository.GetListAsync());

        [HttpGet("list/{groupId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewSubject])]
        public async Task<ActionResult<IEnumerable<SubjectEntity>>> GetListAsync(int groupId)
        {
            if (groupId < 1) return BadRequest();

            GroupEntity? groupEntity = await _groupRepository.GetAsync(groupEntityId: groupId);

            if (groupEntity is null) return NotFound();

            Authorizing authorizing = new(_userRepository, HttpContext);

            if (!authorizing.IsAdminAndSecretarRole())
            {
                bool userAccess = await authorizing.RequireOwnerListAccess
                ([
                    new()
                    {
                        IdList = [groupEntity.CuratorId],
                        Role = Curator
                    },
                    new()
                    {
                        IdList = groupEntity.StudentEntityList.Select(s => s.Id).ToArray(),
                        Role = Student
                    },
                ]);

                if (userAccess is false) return Forbid();
            }

            return Ok(await _subjectRepository.GetListAsync(groupEntityId: groupId));
        }

        [HttpGet("list-by-session")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<SubjectEntity>>> GetListBySessionAsync()
        {
            IEnumerable<SubjectEntity> subjectEntityList = [];

            try
            {
                Guid userIdSession = new HttpContextService(HttpContext).GetUserIdFromCookie();
                HashSet<RoleEnum> roleListUserSession = await _userRepository.GetRoleListAsync(userIdSession);

                if (new Authorizing(_userRepository, HttpContext).IsAdminAndSecretarRole())
                {
                    return Ok(await _subjectRepository.GetListAsync());
                }

                if (roleListUserSession.Contains(Curator))
                {
                    subjectEntityList = subjectEntityList.Concat(await _subjectRepository.GetListByCuratorAsync(userIdSession));
                }

                if (roleListUserSession.Contains(Teacher))
                {
                    subjectEntityList = subjectEntityList.Concat(await _subjectRepository.GetListByTeacherAsync(userIdSession));
                }

                if (roleListUserSession.Contains(Student))
                {
                    subjectEntityList = subjectEntityList.Concat(await _subjectRepository.GetListByStudentAsync(userIdSession));
                }
            }
            catch
            {
                return Unauthorized();
            }

            return Ok(subjectEntityList);
        }

        [HttpGet("{subjectId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewSubject])]
        public async Task<ActionResult<SubjectEntity>> GetAsync(int subjectId)
        {
            if (subjectId < 1) return BadRequest();

            SubjectEntity? subjectEntity = await _subjectRepository.GetAsync(subjectEntityId: subjectId);

            if (subjectEntity is null) return NotFound();

            Authorizing authorizing = new(_userRepository, HttpContext);

            if (!authorizing.IsAdminAndSecretarRole())
            {
                bool userAccess = await new Authorizing(_userRepository, HttpContext).RequireOwnerListAccess
                    ([
                        new()
                        {
                            IdList = [subjectEntity.TeacherId],
                            Role = Teacher
                        },
                        new()
                        {
                            IdList = [_groupRepository.GetAsync(subjectEntity).Result.CuratorId],
                            Role = Curator
                        },
                        new()
                        {
                            IdList = 
                                _groupRepository
                                .GetAsync(subjectEntity).Result.StudentEntityList
                                .Select(s => s.Id)
                                .ToArray(),

                            Role = Student
                        },
                    ]);

                if (userAccess is false) return Forbid();
            }

            return Ok(subjectEntity);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditSubject])]
        public async Task<ActionResult<SubjectDTO>> CreateAsync([FromBody] SubjectDTO subjectDTO)
        {
            SubjectEntity? subjectEntityAnother = await _subjectRepository.GetAsync
                (
                    subjectEntityName: subjectDTO.Name,
                    teacherId: subjectDTO.TeacherId
                );

            if (subjectEntityAnother is not null && subjectEntityAnother.GroupEntityId != subjectDTO.GroupEntityId)
            {
                ModelState.AddModelError("Custom Error", "SubjectEntity already Exists!");

                return BadRequest(ModelState);
            }

            GroupEntity? groupEntity = await _groupRepository.GetAsync(groupEntityId: subjectDTO.GroupEntityId);

            if (groupEntity is null) return NotFound("Group is null!");

            UserEntity? userEntity = await _userRepository.GetNoTrackingAsync(userEntityId: subjectDTO.TeacherId);

            if (userEntity is null) return NotFound("Teacher is null");
            if (!UserService.CheckRoleContains(_userRepository, userEntity, Teacher)) return BadRequest("User is not a Teacher!");

            await _subjectRepository.AddAsync(_subjectRepository.Create(subjectDTO));

            return Created("SubjectEntity", subjectDTO);
        }

        [HttpPut("{subjectId:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditSubject])]
        public async Task<IActionResult> UpdateAsync(int subjectId, [FromBody] SubjectDTO subjectDTO)
        {
            if (subjectId < 1) return BadRequest();

            SubjectEntity? subjectEntityToUpdate = await _subjectRepository.GetAsync(subjectId);

            if (subjectEntityToUpdate is null) return NotFound();

            SubjectEntity? subjectEntityAnother = await _subjectRepository.GetAsync
                (
                    subjectEntityName: subjectDTO.Name,
                    teacherId: subjectDTO.TeacherId
                );

            if (
                subjectEntityAnother is not null &&
                subjectEntityAnother.GroupEntityId != subjectDTO.GroupEntityId &&
                subjectEntityAnother.Id != subjectEntityToUpdate.Id
                )
            {
                ModelState.AddModelError("Custom Error", "SubjectEntity already Exists!");

                return BadRequest(ModelState);
            }

            GroupEntity? groupEntity = await _groupRepository.GetAsync(groupEntityId: subjectDTO.GroupEntityId);

            if (groupEntity is null) return NotFound("Group is null!");

            UserEntity? userEntity = await _userRepository.GetNoTrackingAsync(userEntityId: subjectDTO.TeacherId);

            if (userEntity is null) return NotFound("Teacher is null");
            if (!UserService.CheckRoleContains(_userRepository, userEntity, Teacher)) return BadRequest("User is not a Teacher!");

            await _subjectRepository.UpdateAsync(subjectEntityToUpdate, subjectDTO);

            return NoContent();
        }

        [HttpDelete("{subjectId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditSubject])]
        public async Task<IActionResult> DeleteAsync(int subjectId)
        {
            if (subjectId < 1) return BadRequest();

            SubjectEntity? subjectEntityToRemove = await _subjectRepository.GetAsync(subjectEntityId: subjectId);

            if (subjectEntityToRemove is null) return NotFound();

            await _subjectRepository.RemoveAsync(subjectEntityToRemove);

            return NoContent();
        }
    }
}