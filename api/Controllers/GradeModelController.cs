using api.Data;
using api.Extensions;
using api.Models;
using api.Repositories.Data;
using Microsoft.AspNetCore.Mvc;
using api.Services;
using api.Models.DTO;

using static api.Helpers.Enums.RoleEnum;
using static api.Helpers.Enums.PermissionEnum;
using api.Models.Entities;

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
        public async Task<ActionResult<IEnumerable<GradeModelEntity>>> GetListAsync() => Ok(await _gradeModelRepository.GetListAsync());

        [HttpGet("list-by-student/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewGrade])]
        public async Task<ActionResult<IEnumerable<GradeStudentDTO>>> GetListByStudentIdAsync(Guid id)
        {
            if (await _studentRepository.CheckExistsAsync(id) is false) return NotFound();

            IEnumerable<GradeStudentDTO> gradeList = await _gradeModelRepository.GetListByStudentIdAsync(id);

            // bool userAccess = await new Authorizing(_userRepository, HttpContext).RequireOwnerListAccess
            //     ([
            //         new()
            //         {
            //             IdList = [_subjectRepository.GetAsync(gradeList.ToList()[0].SubjectId).Result!.TeacherId],
            //             Role = Teacher
            //         },
            //         new()
            //         {
            //             IdList = [_groupRepository.GetAsync(_subjectRepository.GetAsync(gradeList.ToList()[0].SubjectId).Result!.GroupId).Result!.CuratorId],
            //             Role = Curator
            //         },
            //         new()
            //         {
            //             IdList = gradeList.Select(grade => grade.StudentId).ToArray(),
            //             Role = Student
            //         },
            //     ]);

            // if (userAccess is false) return Forbid();

            return Ok(gradeList);
        }

        [HttpGet("list-by-subject/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewGrade])]
        public async Task<ActionResult<IEnumerable<GradeModelEntity>>> GetListBySubjectIdAsync(int id)
        {
            if (id < 1) return BadRequest();

            if (await _subjectRepository.CheckExistsAsync(id) is false) return NotFound();

            IEnumerable<GradeModelEntity> gradeList = await _gradeModelRepository.GetListBySubjectIdAsync(id);

            // bool userAccess = await new Authorizing(_userRepository, HttpContext).RequireOwnerListAccess
            //     ([
            //         new()
            //         {
            //             IdList = [_subjectRepository.GetAsync(gradeList.ToList()[0].SubjectId).Result!.TeacherId],
            //             Role = Teacher
            //         },
            //         new()
            //         {
            //             IdList = [_groupRepository.GetAsync(_subjectRepository.GetAsync(gradeList.ToList()[0].SubjectId).Result!.GroupId).Result!.CuratorId],
            //             Role = Curator
            //         },
            //         new()
            //         {
            //             IdList = gradeList.SelectMany(grade => grade.StudentToValueList.Select(sv => sv.StudentId)).ToArray(),
            //             Role = Student
            //         },
            //     ]);

            // if (userAccess is false) return Forbid();

            return Ok(gradeList);
        }

        [HttpGet("list-by-student/{studentId}/by-subject/{subjectId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewGrade])]
        public async Task<ActionResult<IEnumerable<GradeStudentDTO>>> GetListByStudentAndSubjectIdAsync(Guid studentId, int subjectId)
        {
            if (subjectId < 1) return BadRequest();

            if (await _studentRepository.CheckExistsAsync(studentId) is false) return NotFound("Student is null!");
            if (await _subjectRepository.CheckExistsAsync(subjectId) is false) return NotFound("Subject is null!");

            IEnumerable<GradeStudentDTO> gradeList = await _gradeModelRepository.GetListByStudentAndSubjectIdAsync(studentId, subjectId);

            // bool userAccess = await new Authorizing(_userRepository, HttpContext).RequireOwnerListAccess
            //     ([
            //         new()
            //         {
            //             IdList = [_subjectRepository.GetAsync(gradeList.ToList()[0].SubjectId).Result!.TeacherId],
            //             Role = Teacher
            //         },
            //         new()
            //         {
            //             IdList = [_groupRepository.GetAsync(_subjectRepository.GetAsync(gradeList.ToList()[0].SubjectId).Result!.GroupId).Result!.CuratorId],
            //             Role = Curator
            //         },
            //         new()
            //         {
            //             IdList = gradeList.Select(grade => grade.StudentId).ToArray(),
            //             Role = Student
            //         },
            //     ]);

            // if (userAccess is false) return Forbid();

            return Ok(gradeList);
        }

        [HttpGet("student-average/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<double>> GetAverageAsync(Guid id)
        {
            StudentEntity? student = await _studentRepository.GetAsync(id);

            if (student is null) return NotFound("Student is null!");
            if (student.GroupId is null) return NotFound("Group is null!");

            return Ok(await _subjectRepository.GetAverageAsync(student));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditGrade])]
        public async Task<ActionResult<GradeModelDTO>> AddAsync([FromBody] GradeModelDTO gradeDTO)
        {
            if (await _subjectRepository.CheckExistsAsync(gradeDTO.SubjectId) is false) return NotFound("Subject is null!");

            gradeDTO.SetDate = gradeDTO.SetDate.Date;

            GradeModelEntity? gradesEntity = await _gradeModelRepository.GetAsync(gradeDTO.SetDate);

            if (gradesEntity is not null && gradesEntity.Type.Name == gradeDTO.Type.ToString())
            {
                _gradeModelRepository.UpdateGradeList(gradesEntity, gradeDTO.StudentToValueList);
            }

            if (gradesEntity is null)
            {
                await _gradeModelRepository.AddAsync(_gradeModelRepository.Create(gradeDTO));
            }

            return Created("GradeEntity", gradeDTO);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditGrade])]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] GradeModelDTO gradeDTO)
        {
            GradeModelEntity? gradeToUpdate = await _gradeModelRepository.GetAsync(id);
            GradeModelEntity? gradeAnother = await _gradeModelRepository.GetAsync(gradeDTO.SetDate);

            if (gradeToUpdate is null) return NotFound("GradeEntity is null");

            if (await _subjectRepository.CheckExistsAsync(gradeDTO.SubjectId) is false) return NotFound("Subject is null!");

            if (gradeAnother is not null && gradeAnother.Id != gradeToUpdate.Id)
            {
                ModelState.AddModelError("Custom Error", "GradesEntity already Exists!");

                return BadRequest(ModelState);
            }

            await _gradeModelRepository.UpdateAsync(gradeToUpdate, gradeDTO);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditGrade])]
        public async Task<ActionResult> RemoveAsync(Guid id)
        {
            GradeModelEntity? gradeToRemove = await _gradeModelRepository.GetAsync(id);

            if (gradeToRemove is null) return NotFound();

            await _gradeModelRepository.RemoveAsync(gradeToRemove);

            return NoContent();
        }
    }
}