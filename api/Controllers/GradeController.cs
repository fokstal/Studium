using api.Data;
using api.Extensions;
using api.Models;
using api.Model.DTO;
using api.Repositories.Data;
using Microsoft.AspNetCore.Mvc;

using static api.Helpers.Enums.RoleEnum;
using static api.Helpers.Enums.PermissionEnum;
using api.Services;

namespace api.Controllers
{
    [Route("grade")]
    [ApiController]
    [RequireRoles([Admin, Secretar, Curator, Teacher, Student])]
    public class GradeController(AppDbContext db) : ControllerBase
    {
        private readonly GradeRepository _gradeRepository = new(db);
        private readonly StudentRepository _studentRepository = new(db);
        private readonly SubjectRepository _subjectRepository = new(db);
        private readonly GroupRepository _groupRepository = new(db);
        private readonly UserRepository _userRepository = new(db);

        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewGradeList])]
        public async Task<ActionResult<IEnumerable<GradeEntity>>> GetListAsync() => Ok(await _gradeRepository.GetListAsync());

        [HttpGet("list-by-student/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewGrade])]
        public async Task<ActionResult<IEnumerable<GradeEntity>>> GetListByStudentIdAsync(Guid id)
        {
            if (await _studentRepository.CheckExistsAsync(id) is false) return NotFound();

            IEnumerable<GradeEntity> gradeList = await _gradeRepository.GetListByStudentIdAsync(id);

            bool userAccess = await new Authorizing(_userRepository, HttpContext).RequireOwnerListAccess
                ([
                    new()
                    {
                        IdList = [_subjectRepository.GetAsync(gradeList.ToList()[0].SubjectId).Result!.TeacherId],
                        Role = Teacher
                    },
                    new()
                    {
                        IdList = [_groupRepository.GetAsync(_subjectRepository.GetAsync(gradeList.ToList()[0].SubjectId).Result!.GroupId).Result!.CuratorId],
                        Role = Curator
                    },
                    new()
                    {
                        IdList = gradeList.Select(grade => grade.StudentId).ToArray(),
                        Role = Student
                    },
                ]);

            if (userAccess is false) return Forbid();

            return Ok(gradeList);
        }

        [HttpGet("list-by-subject/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewGrade])]
        public async Task<ActionResult<IEnumerable<GradeEntity>>> GetListBySubjectIdAsync(int id)
        {
            if (id < 1) return BadRequest();

            if (await _subjectRepository.CheckExistsAsync(id) is false) return NotFound();

            IEnumerable<GradeEntity> gradeList = await _gradeRepository.GetListBySubjectIdAsync(id);

            bool userAccess = await new Authorizing(_userRepository, HttpContext).RequireOwnerListAccess
                ([
                    new()
                    {
                        IdList = [_subjectRepository.GetAsync(gradeList.ToList()[0].SubjectId).Result!.TeacherId],
                        Role = Teacher
                    },
                    new()
                    {
                        IdList = [_groupRepository.GetAsync(_subjectRepository.GetAsync(gradeList.ToList()[0].SubjectId).Result!.GroupId).Result!.CuratorId],
                        Role = Curator
                    },
                    new()
                    {
                        IdList = gradeList.Select(grade => grade.StudentId).ToArray(),
                        Role = Student
                    },
                ]);

            if (userAccess is false) return Forbid();

            return Ok(gradeList);
        }

        [HttpGet("list-by-student/{studentId:int}/by-subject/{subjectId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewGrade])]
        public async Task<ActionResult<IEnumerable<GradeEntity>>> GetListByStudentAndSubjectIdAsync(Guid studentId, int subjectId)
        {
            if (subjectId < 1) return BadRequest();

            if (await _studentRepository.CheckExistsAsync(studentId) is false) return NotFound("Student is null!");
            if (await _subjectRepository.CheckExistsAsync(subjectId) is false) return NotFound("Subject is null!");

            IEnumerable<GradeEntity> gradeList = await _gradeRepository.GetListByStudentAndSubjectIdAsync(studentId, subjectId);

            bool userAccess = await new Authorizing(_userRepository, HttpContext).RequireOwnerListAccess
                ([
                    new()
                    {
                        IdList = [_subjectRepository.GetAsync(gradeList.ToList()[0].SubjectId).Result!.TeacherId],
                        Role = Teacher
                    },
                    new()
                    {
                        IdList = [_groupRepository.GetAsync(_subjectRepository.GetAsync(gradeList.ToList()[0].SubjectId).Result!.GroupId).Result!.CuratorId],
                        Role = Curator
                    },
                    new()
                    {
                        IdList = gradeList.Select(grade => grade.StudentId).ToArray(),
                        Role = Student
                    },
                ]);

            if (userAccess is false) return Forbid();

            return Ok(gradeList);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditGrade])]
        public async Task<ActionResult<GradeDTO>> AddAsync([FromBody] GradeDTO gradeDTO)
        {
            if (await _studentRepository.CheckExistsAsync(gradeDTO.StudentId) is false) return NotFound("Student is null!");
            if (await _subjectRepository.CheckExistsAsync(gradeDTO.SubjectId) is false) return NotFound("Subject is null!");

            await _gradeRepository.AddAsync(_gradeRepository.Create(gradeDTO));

            return Created("GradeEntity", gradeDTO);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditGrade])]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] GradeDTO gradeDTO)
        {
            if (id < 1) return BadRequest();

            GradeEntity? gradeToUpdate = await _gradeRepository.GetAsync(id);

            if (gradeToUpdate is null) return NotFound("GradeEntity is null");

            if (await _studentRepository.CheckExistsAsync(gradeDTO.StudentId) is false) return NotFound("Student is null!");
            if (await _subjectRepository.CheckExistsAsync(gradeDTO.SubjectId) is false) return NotFound("Subject is null!");

            await _gradeRepository.UpdateAsync(gradeToUpdate, gradeDTO);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditGrade])]
        public async Task<ActionResult> RemoveAsync(int id)
        {
            if (id < 1) return BadRequest();

            GradeEntity? gradeToRemove = await _gradeRepository.GetAsync(id);

            if (gradeToRemove is null) return NotFound();

            await _gradeRepository.RemoveAsync(gradeToRemove);

            return NoContent();
        }
    }
}