using api.Data;
using api.Models;
using api.Model.DTO;
using api.Repositories.Data;
using Microsoft.AspNetCore.Mvc;
using api.Extensions;

using static api.Helpers.Enums.RoleEnum;
using static api.Helpers.Enums.PermissionEnum;

namespace api.Controllers
{
    [Route("subject")]
    [ApiController]
    [RequireRoles([Admin, Secretar, Curator, Teacher, Student])]
    public class SubjectController(AppDbContext db) : ControllerBase
    {
        private readonly SubjectRepository _subjectService = new(db);
        private readonly GroupRepository _groupService = new(db);

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([Read])]
        public async Task<ActionResult<IEnumerable<SubjectEntity>>> GetListAsync() => Ok(await _subjectService.GetListAsync());

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([Read])]
        public async Task<ActionResult<SubjectEntity>> GetByIdAsync(int id)
        {
            if (id < 1) return BadRequest();

            SubjectEntity? subject = await _subjectService.GetAsync(id);

            if (subject is null) return NotFound();

            return Ok(subject);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([Create])]
        public async Task<ActionResult<SubjectDTO>> CreateAsync([FromBody] SubjectDTO subjectDTO)
        {
            if (await _subjectService.GetAsync(subjectDTO.Name, subjectDTO.TeacherId) is not null)
            {
                ModelState.AddModelError("Custom Error", "SubjectEntity already Exists!");

                return BadRequest(ModelState);
            }

            int? groupId = null;

            if (subjectDTO.GroupId is not null)
            {
                GroupEntity? group = await _groupService.GetAsync(subjectDTO.GroupId);

                if (group is null) return NotFound("Group is null!");

                groupId = group.Id;
            }

            await _subjectService.AddAsync(new()
            {
                Name = subjectDTO.Name,
                Descripton = subjectDTO.Descripton,
                TeacherId = subjectDTO.TeacherId,
                GroupId = groupId,
            });

            return Created("SubjectEntity", subjectDTO);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([Update])]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] SubjectDTO subjectDTO)
        {
            if (id < 1) return BadRequest();

            if (await _subjectService.GetAsync(subjectDTO.Name, subjectDTO.TeacherId) is not null)
            {
                ModelState.AddModelError("Custom Error", "SubjectEntity already Exists!");

                return BadRequest(ModelState);
            }

            SubjectEntity? subjectToUpdate = await _subjectService.GetAsync(id);

            if (subjectToUpdate is null) return NotFound();

            int? groupId = null;

            if (subjectDTO.GroupId is not null)
            {
                GroupEntity? group = await _groupService.GetAsync(subjectDTO.GroupId);

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
        [RequirePermissions([Delete])]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id < 1) return BadRequest();

            SubjectEntity? subjectToRemove = await _subjectService.GetAsync(id);

            if (subjectToRemove is null) return NotFound();

            await _subjectService.RemoveAsync(subjectToRemove);

            return NoContent();
        }
    }
}