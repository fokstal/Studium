using api.Data;
using api.Model;
using api.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("subject")]
    [ApiController]
    public class SubjectController(AppDbContext db) : ControllerBase
    {
        private readonly AppDbContext _db = db;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Subject>>> GetListAsync()
        {
            IEnumerable<Subject> subjectList = await _db.Subject.Include(subjectDb => subjectDb.GradeList).ToArrayAsync();

            return Ok(subjectList);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Subject>> GetByIdAsync(int id)
        {
            if (id < 1) return BadRequest();

            Subject? subject = await _db.Subject.Include(subjectDb => subjectDb.GradeList).FirstOrDefaultAsync();

            if (subject is null) return NotFound();

            return Ok(subject);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Subject>> CreateAsync([FromBody] SubjectDTO subjectDTO)
        {
            if (await _db.Subject.FirstOrDefaultAsync
            (
                subjectDb =>
                    subjectDb.Name.ToLower() == subjectDTO.Name.ToLower() &&
                    subjectDb.TeacherName.ToLower() == subjectDTO.TeacherName.ToLower()
            ) is not null)
            {
                ModelState.AddModelError("Custom Error", "Subject already Exists!");

                return BadRequest(ModelState);
            }

            int? groupId = null;

            if (subjectDTO.GroupId is not null)
            {
                Group? group = await _db.Group.FirstOrDefaultAsync(groupDb => groupDb.Id == subjectDTO.GroupId);

                if (group is null) return NotFound("Group is null!");

                groupId = group.Id;
            }

            await _db.Subject.AddAsync(new()
            {
                Name = subjectDTO.Name,
                Descripton = subjectDTO.Descripton,
                TeacherName = subjectDTO.TeacherName,
                GroupId = groupId,
            });

            await _db.SaveChangesAsync();

            return Created("Subject", subjectDTO);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] SubjectDTO subjectDTO)
        {
            if (id < 1) return BadRequest();

            if (await _db.Subject.FirstOrDefaultAsync
            (
                subjectDb =>
                    subjectDb.Name.ToLower() == subjectDTO.Name.ToLower() &&
                    subjectDb.TeacherName.ToLower() == subjectDTO.TeacherName.ToLower()
            ) is not null)
            {
                ModelState.AddModelError("Custom Error", "Subject already Exists!");

                return BadRequest(ModelState);
            }

            Subject? subjectToUpdate = await _db.Subject.FirstOrDefaultAsync(subjectDb => subjectDb.Id == id);

            if (subjectToUpdate is null) return NotFound();

            int? groupId = null;

            if (subjectDTO.GroupId is not null)
            {
                Group? group = await _db.Group.FirstOrDefaultAsync(groupDb => groupDb.Id == subjectDTO.GroupId);

                if (group is null) return NotFound("Group is null!");

                groupId = group.Id;
            }

            subjectToUpdate.Name = subjectDTO.Name;
            subjectToUpdate.Descripton = subjectDTO.Descripton;
            subjectToUpdate.TeacherName = subjectDTO.TeacherName;
            subjectToUpdate.GroupId = groupId;

            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            if (id < 1) return BadRequest();

            Subject? subject = await _db.Subject.FirstOrDefaultAsync();

            if (subject is null) return NotFound();

            _db.Subject.Remove(subject);

            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}