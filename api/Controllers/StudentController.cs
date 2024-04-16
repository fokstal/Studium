using api.Data;
using api.Model;
using api.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetListAsync()
        {
            using (AppDbContext db = new())
            {
                IEnumerable<Student> studentList = await db.Student.ToArrayAsync();

                return Ok(studentList);
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
                Student? student = await db.Student.FirstOrDefaultAsync(studentDb => studentDb.Id == id);

                if (student is null) return NotFound();

                return Ok(student);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateAsync([FromBody] StudentDTO studentDTO)
        {
            using (AppDbContext db = new())
            {
                if (await db.Student.FirstOrDefaultAsync(studentDb => studentDb.PersonId == studentDTO.PersonId && studentDb.GroupId == studentDTO.GroupId) is not null)
                {
                    ModelState.AddModelError("Custom Error", "Student already Exists!");

                    return BadRequest(ModelState);
                }

                Person? person = await db.Person.FirstOrDefaultAsync(personDb => personDb.Id == studentDTO.PersonId);

                if (person is null) return NotFound("Person is null!");

                int? groupId = null;

                if (studentDTO.GroupId is not null)
                {
                    Group? group = await db.Group.FirstOrDefaultAsync(groupDb => groupDb.Id == studentDTO.GroupId);

                    if (group is null) return NotFound("Group is null!");

                    groupId = group.Id;
                }

                await db.Student.AddAsync(new()
                {
                    PersonId = person.Id,
                    GroupId = groupId,
                });

                await db.SaveChangesAsync();

                return Created("Student", studentDTO);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] StudentDTO studentDTO)
        {
            if (id < 1) return BadRequest();

            using (AppDbContext db = new())
            {
                if (await db.Student.FirstOrDefaultAsync(studentDb => studentDb.PersonId == studentDTO.PersonId && studentDb.GroupId == studentDTO.GroupId) is not null)
                {
                    ModelState.AddModelError("Custom Error", "Student already Exists!");

                    return BadRequest(ModelState);
                }

                Student? studentToUpdate = await db.Student.FirstOrDefaultAsync(studentDb => studentDb.Id == id);

                if (studentToUpdate is null) return NotFound();

                Person? person = await db.Person.FirstOrDefaultAsync(personDb => personDb.Id == studentDTO.PersonId);

                if (person is null) return NotFound("Person is null!");

                int? groupId = null;

                if (studentDTO.GroupId is not null)
                {
                    Group? group = await db.Group.FirstOrDefaultAsync(groupDb => groupDb.Id == studentDTO.GroupId);

                    if (group is null) return NotFound("Group is null!");

                    groupId = group.Id;
                }

                studentToUpdate.PersonId = person.Id;
                studentToUpdate.GroupId = groupId;

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
                Student? student = await db.Student.FirstOrDefaultAsync(studentDb => studentDb.Id == id);

                if (student is null) return NotFound();

                db.Student.Remove(student);

                await db.SaveChangesAsync();

                return NoContent();
            }
        }
    }
}