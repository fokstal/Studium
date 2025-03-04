using api.Data;
using api.Extensions;
using api.Models;
using api.Repositories.Data;
using Microsoft.AspNetCore.Mvc;
using api.Services;
using api.Models.DTO;

using static api.Helpers.Enums.RoleEnum;
using static api.Helpers.Enums.PermissionEnum;

namespace api.Controllers
{
    [Route("grade")]
    [ApiController]
    [RequireRoles([Admin, Secretar, Curator, Teacher, Student])]
    public class GradeModelController(AppDbContext db) : ControllerBase
    {
        private readonly GradeModelRepository _gradeModelRepository = new(db);
        private readonly StudentRepository _studentRepository = new(db);
        private readonly SubjectRepository _subjectRepository = new(db);
        private readonly GroupRepository _groupRepository = new(db);
        private readonly UserRepository _userRepository = new(db);

        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewGradeList])]
        public async Task<ActionResult<IEnumerable<GradeModelEntity>>> GetListAsync()
            => Ok(await _gradeModelRepository.GetListAsync());

        [HttpGet("list-by-student/{studentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewGrade])]
        public async Task<ActionResult<IEnumerable<GradeStudentDTO>>> GetListAsync(Guid studentId)
        {
            StudentEntity? studentEntity = await _studentRepository.GetAsync(studentEntityId: studentId);

            if (studentEntity is null) return NotFound();

            IEnumerable<GradeStudentDTO> gradeStudentDTOList = await
                _gradeModelRepository.GetListAsync(studentEntityId: studentId);

            Authorizing authorizing = new(_userRepository, HttpContext);

            if (!authorizing.IsAdminAndSecretarRole())
            {
                bool userAccess = await authorizing.RequireOwnerListAccess
                    ([
                        new()
                    {
                        IdList =
                            _groupRepository
                            .GetAsync(groupEntityId: studentEntity.GroupEntityId)
                            .Result!
                            .SubjectEntityList
                            .Select(s => s.TeacherId)
                            .ToArray(),
                        Role = Teacher
                    },
                    new()
                    {
                        IdList =
                        [
                            _groupRepository
                            .GetAsync(groupEntityId: studentEntity.GroupEntityId)
                            .Result!
                            .CuratorId
                        ],
                        Role = Curator
                    },
                    new()
                    {
                        IdList = [studentId],
                        Role = Student
                    },
                    ]);

                if (userAccess is false) return Forbid();
            }

            return Ok(gradeStudentDTOList);
        }

        [HttpGet("list-by-subject/{subjectId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewGrade])]
        public async Task<ActionResult<IEnumerable<GradeModelEntity>>> GetListAsync(int subjectId)
        {
            if (subjectId < 1) return BadRequest();

            SubjectEntity? subjectEntity = await _subjectRepository.GetAsync(subjectEntityId: subjectId);

            if (subjectEntity is null) return NotFound();

            IEnumerable<GradeModelEntity> gradeModelEntityList = await _gradeModelRepository.GetListAsync(subjectEntityId: subjectId);

            Authorizing authorizing = new(_userRepository, HttpContext);

            if (!authorizing.IsAdminAndSecretarRole())
            {
                bool userAccess = await authorizing.RequireOwnerListAccess
                    ([
                        new()
                        {
                            IdList = [subjectEntity.TeacherId],
                            Role = Teacher
                        },
                        new()
                        {
                            IdList =
                            [
                                _groupRepository
                                .GetAsync(groupEntityId: subjectEntity.GroupEntityId)
                                .Result!
                                .CuratorId
                            ],
                            Role = Curator
                        },
                        new()
                        {
                            IdList =
                                _groupRepository
                                .GetAsync(groupEntityId: subjectEntity.GroupEntityId)
                                .Result!
                                .StudentEntityList
                                .Select(s => s.Id)
                                .ToArray(),
                            Role = Student
                        },
                    ]);

                if (userAccess is false) return Forbid();
            }

            return Ok(gradeModelEntityList);
        }

        [HttpGet("list-by-student/{studentId}/by-subject/{subjectId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewGrade])]
        public async Task<ActionResult<IEnumerable<GradeStudentDTO>>> GetListAsync(Guid studentId, int subjectId)
        {
            if (subjectId < 1) return BadRequest();

            StudentEntity? studentEntity = await _studentRepository.GetAsync(studentEntityId: studentId);
            SubjectEntity? subjectEntity = await _subjectRepository.GetAsync(subjectEntityId: subjectId);


            if (studentEntity is null) return NotFound("Student is null!");
            if (subjectEntity is null) return NotFound("Subject is null!");

            IEnumerable<GradeStudentDTO> gradeStudentDTOList = await
                _gradeModelRepository
                .GetListAsync(studentEntityId: studentId, subjectEntityId: subjectId);

            bool userAccess = await new Authorizing(_userRepository, HttpContext).RequireOwnerListAccess
                ([
                    new()
                    {
                        IdList = [subjectEntity.TeacherId],
                        Role = Teacher
                    },
                    new()
                    {
                        IdList = [_groupRepository.GetAsync(subjectEntity).Result!.CuratorId],
                        Role = Curator
                    },
                    new()
                    {
                        IdList = [studentEntity.Id],
                        Role = Student
                    },
                ]);

            if (userAccess is false) return Forbid();

            return Ok(gradeStudentDTOList);
        }

        [HttpGet("average-by-student/{studentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<double>> GetAverageAsync(Guid studentId)
        {
            StudentEntity? studentEntity = await _studentRepository.GetAsync(studentEntityId: studentId);

            if (studentEntity is null) return NotFound("Student is null!");
            if (studentEntity.GroupEntityId is null) return NotFound("Group is null!");

            return Ok(await _subjectRepository.GetAverageGradeAsync(studentEntity: studentEntity));
        }

        [HttpPost("average-to-subject-list-by-student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<AverageGradeToSubjectDTO>>> GetAverageToSubjectListAsync([FromBody] AverageSemesterGradeDTO averageSemesterGradeDTO)
        {
            StudentEntity? studentEntity = await
                _studentRepository
                .GetAsync(studentEntityId: averageSemesterGradeDTO.StudentEntityId);

            if (studentEntity is null) return NotFound("Student is null!");
            if (studentEntity.GroupEntityId is null) return NotFound("Group is null!");

            return Ok(await _subjectRepository.GetAverageGradeListAsync
            (
                studentEntity: studentEntity,
                startCheck: averageSemesterGradeDTO.StartDate,
                endCheck: averageSemesterGradeDTO.EndDate
            ));
        }

        [HttpPost("average-semester")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<double>> GetAverageAsync([FromBody] AverageSemesterGradeDTO averageSemesterGradeDTO)
        {
            StudentEntity? studentEntity = await
                _studentRepository.GetAsync(studentEntityId: averageSemesterGradeDTO.StudentEntityId);

            if (studentEntity is null) return NotFound("Student is null!");
            if (studentEntity.GroupEntityId is null) return NotFound("Group is null!");

            return Ok(await _subjectRepository.GetAverageGradeAsync
            (
                studentEntity: studentEntity,
                startCheck: averageSemesterGradeDTO.StartDate,
                endCheck: averageSemesterGradeDTO.EndDate
            ));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditGrade])]
        public async Task<ActionResult<GradeModelDTO>> CreateAddAsync([FromBody] GradeModelDTO gradeModelDTO)
        {
            if (await _subjectRepository.CheckExistsAsync(gradeModelDTO.SubjectEntityId) is false)
                return NotFound("Subject is null!");

            if (!await _gradeModelRepository.IsOwnSubjectStudent(gradeModelDTO.SubjectEntityId, gradeModelDTO.GradeDTOList))
                return BadRequest("Students dont have Access to this Subject!");

            gradeModelDTO.SetDate = gradeModelDTO.SetDate.Date;

            // GradeModelEntity? gradeModelEntityControlWork = await
            //     _gradeModelRepository.
            //     GetAsync
            //     (
            //         subjectEntityId: gradeModelDTO.SubjectEntityId,
            //         typeName: GradeTypeEnum.ControlWork.ToString()
            //     );

            // if (gradeModelEntityControlWork is not null && gradeModelEntityControlWork.SetDate != gradeModelDTO.SetDate)
            // {
            //     return BadRequest("ControlWork for this Subject already Exists!");
            // }

            GradeModelEntity? gradeModelEntity = await
                _gradeModelRepository.
                GetAsync
                (
                    setDate: gradeModelDTO.SetDate,
                    subjectEntityId: gradeModelDTO.SubjectEntityId,
                    typeName: gradeModelDTO.TypeEnum.ToString()
                );

            if (gradeModelEntity is not null) await _gradeModelRepository.UpdateGradeListAsync(gradeModelEntity, gradeModelDTO);

            if (gradeModelEntity is null) await _gradeModelRepository.AddAsync(_gradeModelRepository.Create(gradeModelDTO));

            return Created("GradeEntity", gradeModelDTO);
        }

        [HttpPut("{studentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditGrade])]
        public async Task<IActionResult> UpdateAsync(Guid studentId, [FromBody] GradeModelDTO gradeModelDTO)
        {
            GradeModelEntity? gradeModelToUpdate = await _gradeModelRepository.GetAsync(studentEntityId: studentId);
            GradeModelEntity? gradeModelInDate = await _gradeModelRepository.GetAsync(setDate: gradeModelDTO.SetDate);

            if (gradeModelToUpdate is null) return NotFound("GradeEntity is null");

            if (await _subjectRepository.CheckExistsAsync(gradeModelDTO.SubjectEntityId) is false)
                return NotFound("Subject is null!");

            if (gradeModelInDate is not null && gradeModelInDate.Id != gradeModelToUpdate.Id)
            {
                ModelState.AddModelError("Custom Error", "GradesEntity already Exists!");

                return BadRequest(ModelState);
            }

            await _gradeModelRepository.UpdateAsync(gradeModelToUpdate, gradeModelDTO);

            return NoContent();
        }

        [HttpDelete("{studentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditGrade])]
        public async Task<ActionResult> RemoveAsync(Guid studentId)
        {
            GradeModelEntity? gradeToRemove = await _gradeModelRepository.GetAsync(studentEntityId: studentId);

            if (gradeToRemove is null) return NotFound();

            await _gradeModelRepository.RemoveAsync(gradeToRemove);

            return NoContent();
        }
    }
}