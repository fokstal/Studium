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
    [Route("group")]
    [ApiController]
    [RequireRoles([Admin, Secretar, Curator, Teacher, Student])]
    public class GroupEntityController(AppDbContext db) : ControllerBase
    {
        private readonly GroupRepository _groupRepository = new(db);

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([Read])]
        public async Task<ActionResult<IEnumerable<GroupEntity>>> GetListAsync() => Ok(await _groupRepository.GetListAsync());

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([Read])]
        public async Task<ActionResult<GroupEntity>> GetAsync(int id)
        {
            if (id < 1) return BadRequest();

            GroupEntity? group = await _groupRepository.GetAsync(id);

            if (group is null) return NotFound();

            return Ok(group);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([Create])]
        public async Task<ActionResult<GroupDTO>> CreateAsync([FromBody] GroupDTO groupDTO)
        {
            if (await _groupRepository.GetAsync(groupDTO.Name) is not null)
            {
                ModelState.AddModelError("Custom Error", "GroupEntity already Exists!");

                return BadRequest(ModelState);
            }

            await _groupRepository.AddAsync(new()
            {
                Name = groupDTO.Name,
                Description = groupDTO.Description,
                CuratorId = groupDTO.CuratorId,
                AuditoryName = groupDTO.AuditoryName,
            });

            return Created("GroupEntity", groupDTO);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([Update])]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] GroupDTO groupDTO)
        {
            if (id < 1) return BadRequest();

            if (await _groupRepository.GetAsync(groupDTO.Name) is not null)
            {
                ModelState.AddModelError("Custom Error", "GroupEntity already Exists!");

                return BadRequest(ModelState);
            }

            GroupEntity? groupToUpdate = await _groupRepository.GetAsync(id);

            if (groupToUpdate is null) return NotFound();

            await _groupRepository.UpdateAsync(groupToUpdate, groupDTO);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([Delete])]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id < 1) return BadRequest();

            GroupEntity? groupToRemove = await _groupRepository.GetAsync(id);

            if (groupToRemove is null) return NotFound();

            await _groupRepository.RemoveAsync(groupToRemove);

            return NoContent();
        }
    }
}