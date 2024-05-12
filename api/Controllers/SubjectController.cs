using api.Data;
using api.Model;
using api.Model.DTO;
using api.Services.DataServices;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("subject")]
    [ApiController]
    public class SubjectController(AppDbContext db) : ControllerBase
    {
        private readonly SubjectService _subjectService = new(db);
        private readonly GroupService _groupService = new(db);

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Subject>>> GetListAsync() => Ok(await _subjectService.GetListAsync());

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Subject>> GetByIdAsync(int id)
        {
            if (id < 1) return BadRequest();

            Subject? subject = await _subjectService.GetAsync(id);

            if (subject is null) return NotFound();

            return Ok(subject);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SubjectDTO>> CreateAsync([FromBody] SubjectDTO subjectDTO)
        {
            if (await _subjectService.GetAsync(subjectDTO.Name, subjectDTO.TeacherName) is not null)
            {
                ModelState.AddModelError("Custom Error", "Subject already Exists!");

                return BadRequest(ModelState);
            }

            int? groupId = null;

            if (subjectDTO.GroupId is not null)
            {
                Group? group = await _groupService.GetAsync(subjectDTO.GroupId);

                if (group is null) return NotFound("Group is null!");

                groupId = group.Id;
            }

            await _subjectService.AddAsync(new()
            {
                Name = subjectDTO.Name,
                Descripton = subjectDTO.Descripton,
                TeacherName = subjectDTO.TeacherName,
                GroupId = groupId,
            });

            return Created("Subject", subjectDTO);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] SubjectDTO subjectDTO)
        {
            if (id < 1) return BadRequest();

            if (await _subjectService.GetAsync(subjectDTO.Name, subjectDTO.TeacherName) is not null)
            {
                ModelState.AddModelError("Custom Error", "Subject already Exists!");

                return BadRequest(ModelState);
            }

            Subject? subjectToUpdate = await _subjectService.GetAsync(id);

            if (subjectToUpdate is null) return NotFound();

            int? groupId = null;

            if (subjectDTO.GroupId is not null)
            {
                Group? group = await _groupService.GetAsync(subjectDTO.GroupId);

                if (group is null) return NotFound("Group is null!");

                groupId = group.Id;
            }

            subjectDTO.GroupId = groupId;

            await _subjectService.UpdateAsync(subjectToUpdate, subjectDTO);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id < 1) return BadRequest();

            Subject? subjectToRemove = await _subjectService.GetAsync(id);

            if (subjectToRemove is null) return NotFound();

            await _subjectService.RemoveAsync(subjectToRemove);

            return NoContent();
        }
    }
}