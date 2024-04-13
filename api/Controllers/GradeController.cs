using api.Data;
using api.Model;
using api.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("grade")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetGradeListAsync()
        {
            using (AppDbContext db = new())
            {

                IEnumerable<Grade> gradeList = await db.Grade.ToArrayAsync();

                return Ok(gradeList);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetGradeAsync([FromBody] GradeDTO gradeDTO)
        {
            if (gradeDTO.StudentId < 1 || gradeDTO.SubjectId < 1) return BadRequest();

            using (AppDbContext db = new())
            {
                Grade? grade = await db.Grade.FirstOrDefaultAsync
                    (gradeDb =>
                        gradeDb.StudentId == gradeDTO.StudentId &&
                        gradeDb.SubjectId == gradeDTO.SubjectId
                    );

                if (grade is null) return NotFound();

                return Ok(grade);
            }
        }

        [HttpGet("by-student/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetGradeListByStudentIdAsync(int id)
        {
            if (id < 1) return BadRequest();

            using (AppDbContext db = new())
            {
                if (await db.Student.FirstOrDefaultAsync(studentDb => studentDb.Id == id) is null) return NotFound();

                IEnumerable<Grade> gradeListByStudentId = db.Grade.Where(gradeDb => gradeDb.StudentId == id);

                return Ok(gradeListByStudentId);
            }
        }

        [HttpGet("by-subject/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetGradeListBySubjectIdAsync(int id)
        {
            if (id < 1) return BadRequest();

            using (AppDbContext db = new())
            {
                if (await db.Subject.FirstOrDefaultAsync(subjectDb => subjectDb.Id == id) is null) return NotFound();

                IEnumerable<Grade> gradeListBySubjectId = db.Grade.Where(gradeDb => gradeDb.SubjectId == id);

                return Ok(gradeListBySubjectId);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddGrade([FromBody] GradeDTO gradeDTO)
        {
            using (AppDbContext db = new())
            {
                Student? student = await db.Student.FirstOrDefaultAsync(studentDb => studentDb.Id == gradeDTO.StudentId);

                if (student is null) return NotFound("Student is null!");

                Subject? subject = await db.Subject.FirstOrDefaultAsync(subjectDb => subjectDb.Id == gradeDTO.SubjectId);

                if (subject is null) return NotFound("Subject is null!");

                await db.Grade.AddAsync(new()
                {
                    Value = gradeDTO.Value,
                    StudentId = gradeDTO.StudentId,
                    Student = student,
                    SubjectId = gradeDTO.SubjectId,
                    Subject = subject,
                    SetDate = DateTime.Now,
                });

                await db.SaveChangesAsync();

                return CreatedAtAction(nameof(GetGradeAsync), gradeDTO);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateGrade([FromBody] GradeDTO gradeDTO, [FromBody] GradeDTO gradeDTONew)
        {
            using (AppDbContext db = new())
            {
                Grade? gradeToUpdate = await db.Grade.FirstOrDefaultAsync
                    (
                        gradeDb =>
                            gradeDb.StudentId == gradeDTO.StudentId &&
                            gradeDb.SubjectId == gradeDTO.SubjectId
                    );

                if (gradeToUpdate is null) return NotFound("Grade is null");

                Student? student = await db.Student.FirstOrDefaultAsync(studentDb => studentDb.Id == gradeDTONew.StudentId);

                if (student is null) return NotFound("Student is null!");

                Subject? subject = await db.Subject.FirstOrDefaultAsync(subjectDb => subjectDb.Id == gradeDTONew.SubjectId);

                if (subject is null) return NotFound("Subject is null!");

                gradeToUpdate.Value = gradeDTONew.Value;
                gradeToUpdate.SetDate = DateTime.Now;
                gradeToUpdate.StudentId = student.Id;
                gradeToUpdate.Student = student;
                gradeToUpdate.SubjectId = subject.Id;
                gradeToUpdate.Subject = subject;

                await db.SaveChangesAsync();

                return NoContent();
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteGradeAsync([FromBody] GradeDTO gradeDTO)
        {
            if (gradeDTO.StudentId < 1 || gradeDTO.SubjectId < 1) return BadRequest();

            using (AppDbContext db = new())
            {
                Grade? grade = await db.Grade.FirstOrDefaultAsync
                    (gradeDb =>
                        gradeDb.StudentId == gradeDTO.StudentId &&
                        gradeDb.SubjectId == gradeDTO.SubjectId
                    );

                if (grade is null) return NotFound();

                db.Grade.Remove(grade);

                await db.SaveChangesAsync();

                return NoContent();
            }
        }
    }
}