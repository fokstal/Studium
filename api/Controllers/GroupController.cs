using api.Data;
using api.Model;
using api.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("group")]
    [ApiController]
    public class GroupController(AppDbContext db) : ControllerBase
    {
        private readonly AppDbContext _db = db;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Group>>> GetListAsync()
        {
            IEnumerable<Group> groupList = await _db.Group.Include(group_db => group_db.StudentList).Include(group_db => group_db.SubjectList).ToArrayAsync();

            return Ok(groupList);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Group>> GetAsync(int id)
        {
            if (id < 1) return BadRequest();

            Group? group = await _db.Group.Include(group_db => group_db.StudentList).Include(group_db => group_db.SubjectList).FirstOrDefaultAsync(group_db => group_db.Id == id);

            if (group is null) return NotFound();

            return Ok(group);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Group>> CreateAsync([FromBody] GroupDTO groupDTO)
        {
            if (await _db.Group.FirstOrDefaultAsync(group_db => group_db.Name.ToLower() == groupDTO.Name.ToLower()) is not null)
            {
                ModelState.AddModelError("Custom Error", "Group already Exists!");

                return BadRequest(ModelState);
            }

            await _db.Group.AddAsync(new()
            {
                Name = groupDTO.Name,
                Description = groupDTO.Description,
                Curator = groupDTO.Curator,
                AuditoryName = groupDTO.AuditoryName,
            });

            await _db.SaveChangesAsync();

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

            if (await _db.Group.FirstOrDefaultAsync(group_db => group_db.Name.ToLower() == groupDTO.Name.ToLower()) is not null)
            {
                ModelState.AddModelError("Custom Error", "Group already Exists!");

                return BadRequest(ModelState);
            }

            Group? groupToUpdate = await _db.Group.Include(group_db => group_db.StudentList).Include(group_db => group_db.SubjectList).FirstOrDefaultAsync(group_db => group_db.Id == id);

            if (groupToUpdate is null) return NotFound();

            groupToUpdate.Name = groupDTO.Name;
            groupToUpdate.Description = groupDTO.Name;
            groupToUpdate.Curator = groupDTO.Curator;
            groupToUpdate.AuditoryName = groupDTO.AuditoryName;

            await _db.SaveChangesAsync();

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

            Group? group = await _db.Group.Include(group_db => group_db.StudentList).Include(group_db => group_db.SubjectList).FirstOrDefaultAsync(group_db => group_db.Id == id);

            if (group is null) return NotFound();

            _db.Group.Remove(group);

            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}