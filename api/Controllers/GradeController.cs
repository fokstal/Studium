using api.Data;
using api.Extensions;
using api.Models;
using api.Model.DTO;
using api.Repositories.Data;
using Microsoft.AspNetCore.Mvc;

using static api.Helpers.Enums.RoleEnum;
using static api.Helpers.Enums.PermissionEnum;

namespace api.Controllers
{
    [Route("grade")]
    [ApiController]
    [RequireRoles([Admin, Secretar, Curator, Teacher, Student])]
    public class GradeController(AppDbContext db) : ControllerBase
    {
        private readonly StudentRepository _studentRepository = new(db);
        private readonly SubjectRepository _subjectRepository = new(db);
        private readonly GradeRepository _gradeRepository = new(db);

        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([Read])]
        public async Task<ActionResult<IEnumerable<GradeEntity>>> GetListAsync() => Ok(await _gradeRepository.GetListAsync());

        [HttpGet("list-by-student/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([Read])]
        public async Task<ActionResult<IEnumerable<GradeEntity>>> GetListByStudentIdAsync(int id)
        {
            if (id < 1) return BadRequest();

            if (await _studentRepository.CheckExistsAsync(id) is false) return NotFound();

            return Ok(await _gradeRepository.GetListByStudentIdAsync(id));
        }

        [HttpGet("list-by-subject/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([Read])]
        public async Task<ActionResult<IEnumerable<GradeEntity>>> GetListBySubjectIdAsync(int id)
        {
            if (id < 1) return BadRequest();

            if (await _subjectRepository.CheckExistsAsync(id) is false) return NotFound();

            return Ok(await _gradeRepository.GetListBySubjectIdAsync(id));
        }

        [HttpGet("list-by-student/{studentId:int}/by-subject/{subjectId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([Read])]
        public async Task<ActionResult<IEnumerable<GradeEntity>>> GetListByStudentAndSubjectIdAsync(int studentId, int subjectId)
        {
            if (studentId < 1 || subjectId < 1) return BadRequest();

            if (await _studentRepository.CheckExistsAsync(studentId) is false) return NotFound("Student is null!");
            if (await _subjectRepository.CheckExistsAsync(subjectId) is false) return NotFound("Subject is null!");

            return Ok(await _gradeRepository.GetListByStudentAndSubjectIdAsync(studentId, subjectId));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([Create])]
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
        [RequirePermissions([Update])]
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
        [RequirePermissions([Delete])]
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