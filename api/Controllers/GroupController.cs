using api.Data;
using api.Model;
using api.Model.DTO;
using api.Services.DataServices;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("group")]
    [ApiController]
    public class GroupController(AppDbContext db) : ControllerBase
    {
        private readonly GroupService _groupService = new(db);

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Group>>> GetListAsync() => Ok(await _groupService.GetListAsync());

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Group>> GetAsync(int id)
        {
            if (id < 1) return BadRequest();

            Group? group = await _groupService.GetAsync(id);

            if (group is null) return NotFound();

            return Ok(group);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Group>> CreateAsync([FromBody] GroupDTO groupDTO)
        {
            if (await _groupService.GetAsync(groupDTO.Name) is not null)
            {
                ModelState.AddModelError("Custom Error", "Group already Exists!");

                return BadRequest(ModelState);
            }

            await _groupService.AddAsync(new()
            {
                Name = groupDTO.Name,
                Description = groupDTO.Description,
                Curator = groupDTO.Curator,
                AuditoryName = groupDTO.AuditoryName,
            });

            return Created("Group", groupDTO);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] GroupDTO groupDTO)
        {
            if (id < 1) return BadRequest();

            if (await _groupService.GetAsync(groupDTO.Name) is not null)
            {
                ModelState.AddModelError("Custom Error", "Group already Exists!");

                return BadRequest(ModelState);
            }

            Group? groupToUpdate = await _groupService.GetAsync(id);

            if (groupToUpdate is null) return NotFound();

            await _groupService.UpdateAsync(groupToUpdate, groupDTO);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            if (id < 1) return BadRequest();

            Group? groupToRemove = await _groupService.GetAsync(id);

            if (groupToRemove is null) return NotFound();

            await _groupService.RemoveAsync(groupToRemove);

            return NoContent();
        }
    }
}