using api.Data;
using api.Model;
using api.Model.DTO;
using api.Services.DataServices;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("grade")]
    [ApiController]
    public class GradeController(AppDbContext db) : ControllerBase
    {
        private readonly StudentService _studentService = new(db);
        private readonly SubjectService _subjectService = new(db);
        private readonly GradeService _gradeService = new(db);

        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Grade>>> GetListAsync() => Ok(await _gradeService.GetListAsync());

        [HttpGet("list-by-student/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Grade>>> GetListByStudentIdAsync(int id)
        {
            if (id < 1) return BadRequest();

            if (await _studentService.CheckExistsAsync(id) is false) return NotFound();

            return Ok(await _gradeService.GetListByStudentIdAsync(id));
        }

        [HttpGet("list-by-subject/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Grade>>> GetListBySubjectIdAsync(int id)
        {
            if (id < 1) return BadRequest();

            if (await _subjectService.CheckExistsAsync(id) is false) return NotFound();

            return Ok(await _gradeService.GetListBySubjectIdAsync(id));
        }

        [HttpGet("list-by-student/{studentId:int}/by-subject/{subjectId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Grade>>> GetListByStudentAndSubjectIdAsync(int studentId, int subjectId)
        {
            if (studentId < 1 || subjectId < 1) return BadRequest();

            if (await _studentService.CheckExistsAsync(studentId) is false) return NotFound("Student is null!");
            if (await _subjectService.CheckExistsAsync(subjectId) is false) return NotFound("Subject is null!");

            return Ok(await _gradeService.GetListByStudentAndSubjectIdAsync(studentId, subjectId));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GradeDTO>> AddAsync([FromBody] GradeDTO gradeDTO)
        {
            if (await _studentService.CheckExistsAsync(gradeDTO.StudentId) is false) return NotFound("Student is null!");
            if (await _subjectService.CheckExistsAsync(gradeDTO.SubjectId) is false) return NotFound("Subject is null!");

            await _gradeService.AddAsync(new()
            {
                Value = gradeDTO.Value,
                StudentId = gradeDTO.StudentId,
                SubjectId = gradeDTO.SubjectId,
                SetDate = DateTime.Now,
            });

            return Created("Grade", gradeDTO);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] GradeDTO gradeDTO)
        {
            if (id < 1) return BadRequest();

            Grade? gradeToUpdate = await _gradeService.GetAsync(id);

            if (gradeToUpdate is null) return NotFound("Grade is null");

            if (await _studentService.CheckExistsAsync(gradeDTO.StudentId) is false) return NotFound("Student is null!");
            if (await _subjectService.CheckExistsAsync(gradeDTO.SubjectId) is false) return NotFound("Subject is null!");

            await _gradeService.UpdateAsync(gradeToUpdate, gradeDTO);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RemoveAsync(int id)
        {
            if (id < 1) return BadRequest();

            Grade? gradeToRemove = await _gradeService.GetAsync(id);

            if (gradeToRemove is null) return NotFound();

            await _gradeService.RemoveAsync(gradeToRemove);

            return NoContent();
        }
    }
}