using api.Data;
using api.Model;
using api.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("subject")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetListAsync()
        {
            using (AppDbContext db = new())
            {
                IEnumerable<Subject> subjectList = await db.Subject.Include(subjectDb => subjectDb.GradeList).ToArrayAsync();

                return Ok(subjectList);
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAsync(int id)
        {
            if (id < 1) return BadRequest();

            using (AppDbContext db = new())
            {
                Subject? subject = await db.Subject.Include(subjectDb => subjectDb.GradeList).FirstOrDefaultAsync();

                if (subject is null) return NotFound();

                return Ok(subject);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateAsync([FromBody] SubjectDTO subjectDTO)
        {
            using (AppDbContext db = new())
            {
                if (await db.Subject.FirstOrDefaultAsync
                (
                    subjectDb =>
                        subjectDb.Name.ToLower() == subjectDTO.Name.ToLower() &&
                        subjectDb.TeacherName.ToLower() == subjectDTO.TeacherName.ToLower()
                ) is not null)
                {
                    ModelState.AddModelError("Custom Error", "Subject already Exists!");

                    return BadRequest(ModelState);
                }

                await db.Subject.AddAsync(new()
                {
                    Name = subjectDTO.Name,
                    Descripton = subjectDTO.Descripton,
                    TeacherName = subjectDTO.TeacherName,
                });

                await db.SaveChangesAsync();

                return Created("Subject", subjectDTO);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] SubjectDTO subjectDTO)
        {
            if (id < 1) return BadRequest();

            using (AppDbContext db = new())
            {
                if (await db.Subject.FirstOrDefaultAsync
                (
                    subjectDb =>
                        subjectDb.Name.ToLower() == subjectDTO.Name.ToLower() &&
                        subjectDb.TeacherName.ToLower() == subjectDTO.TeacherName.ToLower()
                ) is not null)
                {
                    ModelState.AddModelError("Custom Error", "Subject already Exists!");

                    return BadRequest(ModelState);
                }

                Subject? subjectToUpdate = await db.Subject.FirstOrDefaultAsync(subjectDb => subjectDb.Id == id);

                if (subjectToUpdate is null) return NotFound();

                subjectToUpdate.Name = subjectDTO.Name;
                subjectToUpdate.Descripton = subjectDTO.Descripton;
                subjectToUpdate.TeacherName = subjectDTO.TeacherName;

                await db.SaveChangesAsync();

                return NoContent();
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            if (id < 1) return BadRequest();

            using (AppDbContext db = new())
            {
                Subject? subject = await db.Subject.FirstOrDefaultAsync();

                if (subject is null) return NotFound();

                db.Subject.Remove(subject);

                await db.SaveChangesAsync();

                return NoContent();
            }
        }
    }
}