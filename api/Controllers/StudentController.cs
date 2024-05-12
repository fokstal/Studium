using api.Data;
using api.Model;
using api.Model.DTO;
using api.Services.DataServices;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("student")]
    [ApiController]
    public class StudentController(AppDbContext db) : ControllerBase
    {
        private readonly StudentService _studentService = new(db);
        private readonly PersonService _personService = new(db);
        private readonly GroupService _groupService = new(db);

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Student>>> GetListAsync() => Ok(await _studentService.GetListAsync());

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Student>> GetAsync(int id)
        {
            if (id < 1) return BadRequest();

            Student? student = await _studentService.GetAsync(id);

            if (student is null) return NotFound();

            return Ok(student);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Student>> CreateAsync([FromBody] StudentDTO studentDTO)
        {
            if (await _studentService.GetAsync(studentDTO.PersonId, studentDTO.GroupId) is not null)
            {
                ModelState.AddModelError("Custom Error", "Student already Exists!");

                return BadRequest(ModelState);
            }

            Person? person = await _personService.GetAsync(studentDTO.PersonId);

            if (person is null) return NotFound("Person is null!");

            int? groupId = null;

            if (studentDTO.GroupId is not null)
            {
                Group? group = await _groupService.GetAsync(studentDTO.GroupId);

                if (group is null) return NotFound("Group is null!");

                groupId = group.Id;
            }

            studentDTO.GroupId = groupId;

            await _studentService.AddAsync(new()
            {
                PersonId = person.Id,
                GroupId = groupId,
            });

            return Created("Student", studentDTO);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] StudentDTO studentDTO)
        {
            if (id < 1) return BadRequest();

            if (await _studentService.GetAsync(studentDTO.PersonId, studentDTO.GroupId) is not null)
            {
                ModelState.AddModelError("Custom Error", "Student already Exists!");

                return BadRequest(ModelState);
            }

            Student? studentToUpdate = await _studentService.GetAsync(id);

            if (studentToUpdate is null) return NotFound();

            Person? person = await _personService.GetAsync(studentDTO.PersonId);

            if (person is null) return NotFound("Person is null!");

            int? groupId = null;

            if (studentDTO.GroupId is not null)
            {
                Group? group = await _groupService.GetAsync(studentDTO.GroupId);

                if (group is null) return NotFound("Group is null!");

                groupId = group.Id;
            }

            studentDTO.GroupId = groupId;

            await _studentService.UpdateAsync(studentToUpdate, studentDTO);

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

            Student? studentToRemove = await _studentService.GetAsync(id);

            if (studentToRemove is null) return NotFound();

            await _studentService.RemoveAsync(studentToRemove);

            return NoContent();
        }
    }
}