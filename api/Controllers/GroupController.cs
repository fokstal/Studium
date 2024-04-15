using api.Data;
using api.Model;
using api.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("group")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetListAsync()
        {
            using (AppDbContext db = new())
            {
                IEnumerable<Group> groupList = await db.Group.Include(groupDb => groupDb.StudentList).Include(groupDb => groupDb.SubjectList).ToArrayAsync();

                return Ok(groupList);
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
                Group? group = await db.Group.Include(groupDb => groupDb.StudentList).Include(groupDb => groupDb.SubjectList).FirstOrDefaultAsync(groupDb => groupDb.Id == id);

                if (group is null) return NotFound();

                return Ok(group);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateAsync([FromBody] GroupDTO groupDTO)
        {
            using (AppDbContext db = new())
            {
                if (await db.Group.FirstOrDefaultAsync(groupDb => groupDb.Name.ToLower() == groupDTO.Name.ToLower()) is not null)
                {
                    ModelState.AddModelError("Custom Error", "Group already Exists!");

                    return BadRequest(ModelState);
                }

                await db.Group.AddAsync(new()
                {
                    Name = groupDTO.Name,
                    Description = groupDTO.Description,
                    Curator = groupDTO.Curator,
                    AuditoryName = groupDTO.AuditoryName,
                });

                await db.SaveChangesAsync();

                return Created("Group", groupDTO);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] GroupDTO groupDTO)
        {
            if (id < 1) return BadRequest();

            using (AppDbContext db = new())
            {
                if (await db.Group.FirstOrDefaultAsync(groupDb => groupDb.Name.ToLower() == groupDTO.Name.ToLower()) is not null)
                {
                    ModelState.AddModelError("Custom Error", "Group already Exists!");

                    return BadRequest(ModelState);
                }

                Group? groupToUpdate = await db.Group.Include(groupDb => groupDb.StudentList).Include(groupDb => groupDb.SubjectList).FirstOrDefaultAsync(groupDb => groupDb.Id == id);

                if (groupToUpdate is null) return NotFound();

                groupToUpdate.Name = groupDTO.Name;
                groupToUpdate.Description = groupDTO.Name;
                groupToUpdate.Curator = groupDTO.Curator;
                groupToUpdate.AuditoryName = groupDTO.AuditoryName;

                await db.SaveChangesAsync();

                return NoContent();
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            if (id < 1) return BadRequest();

            using (AppDbContext db = new())
            {
                Group? group = await db.Group.Include(groupDb => groupDb.StudentList).Include(groupDb => groupDb.SubjectList).FirstOrDefaultAsync(groupDb => groupDb.Id == id);

                if (group is null) return NotFound();

                db.Group.Remove(group);

                await db.SaveChangesAsync();

                return NoContent();
            }
        }
    }
}