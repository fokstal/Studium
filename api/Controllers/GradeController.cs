using api.Data;
using api.Model;
using api.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("grade")]
    [ApiController]
    public class GradeController(AppDbContext db) : ControllerBase
    {
        private readonly AppDbContext _db = db;

        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Grade>>> GetListAsync()
        {
            IEnumerable<Grade> gradeList = await _db.Grade.ToArrayAsync();

            return Ok(gradeList);
        }

        [HttpGet("list-by-student/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Grade>>> GetByStudentIdAsync(int id)
        {
            if (id < 1) return BadRequest();

            if (await _db.Student.FirstOrDefaultAsync(student_db => student_db.Id == id) is null) return NotFound();

            IEnumerable<Grade> gradeListByStudentId = await _db.Grade.Where(grade_db => grade_db.StudentId == id).ToListAsync();

            return Ok(gradeListByStudentId);
        }

        [HttpGet("list-by-subject/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Grade>>> GetBySubjectIdAsync(int id)
        {
            if (id < 1) return BadRequest();

            if (await _db.Subject.FirstOrDefaultAsync(subject_db => subject_db.Id == id) is null) return NotFound();

            IEnumerable<Grade> gradeListBySubjectId = await _db.Grade.Where(grade_db => grade_db.SubjectId == id).ToListAsync();

            return Ok(gradeListBySubjectId);
        }

        [HttpGet("list-by-student/{studentId:int}/by-subject/{subjectId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Grade>>> GetByStudentAndSubjectIdAsync(int studentId, int subjectId)
        {
            if (studentId < 1 || subjectId < 1) return BadRequest();

            if (await _db.Student.FirstOrDefaultAsync(student_db => student_db.Id == studentId) is null) return NotFound("Student is null!");
            if (await _db.Subject.FirstOrDefaultAsync(subject_db => subject_db.Id == subjectId) is null) return NotFound("Subject is null!");

            IEnumerable<Grade> gradeList = await _db.Grade.Where(grade_db => grade_db.StudentId == studentId && grade_db.SubjectId == subjectId).ToListAsync();

            return Ok(gradeList);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Grade>> AddAsync([FromBody] GradeDTO gradeDTO)
        {
            Student? student = await _db.Student.FirstOrDefaultAsync(student_db => student_db.Id == gradeDTO.StudentId);

            if (student is null) return NotFound("Student is null!");

            Subject? subject = await _db.Subject.FirstOrDefaultAsync(subject_db => subject_db.Id == gradeDTO.SubjectId);

            if (subject is null) return NotFound("Subject is null!");

            await _db.Grade.AddAsync(new()
            {
                Value = gradeDTO.Value,
                StudentId = gradeDTO.StudentId,
                SubjectId = gradeDTO.SubjectId,
                SetDate = DateTime.Now,
            });

            await _db.SaveChangesAsync();

            return Created("Grade", gradeDTO);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync([FromBody] GradeDTO gradeDTO)
        {
            Grade? gradeToUpdate = await _db.Grade.FirstOrDefaultAsync
                (
                    grade_db =>
                        grade_db.StudentId == gradeDTO.StudentId &&
                        grade_db.SubjectId == gradeDTO.SubjectId
                );

            if (gradeToUpdate is null) return NotFound("Grade is null");

            Student? student = await _db.Student.FirstOrDefaultAsync(student_db => student_db.Id == gradeDTO.StudentId);

            if (student is null) return NotFound("Student is null!");

            Subject? subject = await _db.Subject.FirstOrDefaultAsync(subject_db => subject_db.Id == gradeDTO.SubjectId);

            if (subject is null) return NotFound("Subject is null!");

            gradeToUpdate.Value = gradeDTO.Value;
            gradeToUpdate.SetDate = DateTime.Now;

            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAsync([FromBody] GradeDTO gradeDTO)
        {
            if (gradeDTO.StudentId < 1 || gradeDTO.SubjectId < 1) return BadRequest();

            Grade? grade = await _db.Grade.FirstOrDefaultAsync
                (grade_db =>
                    grade_db.StudentId == gradeDTO.StudentId &&
                    grade_db.SubjectId == gradeDTO.SubjectId
                );

            if (grade is null) return NotFound();

            _db.Grade.Remove(grade);

            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}