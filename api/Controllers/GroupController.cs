using api.Data;
using api.Models;
using api.Model.DTO;
using api.Repositories.Data;
using Microsoft.AspNetCore.Mvc;
using api.Extensions;

using static api.Helpers.Enums.RoleEnum;
using static api.Helpers.Enums.PermissionEnum;
using api.Services;

namespace api.Controllers
{
    [Route("group")]
    [ApiController]
    [RequireRoles([Admin, Secretar, Curator, Teacher, Student])]
    public class GroupController(AppDbContext db) : ControllerBase
    {
        private readonly GroupRepository _groupRepository = new(db);
        private readonly UserRepository _userRepository = new(db);


        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewGroup])]
        public async Task<ActionResult<IEnumerable<GroupEntity>>> GetListAsync() 
            => Ok(await _groupRepository.GetListAsync());

        [HttpGet("{groupId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([ViewGroup])]
        public async Task<ActionResult<GroupEntity>> GetAsync(int groupId)
        {
            if (groupId < 1) return BadRequest();

            GroupEntity? groupEntity = await _groupRepository.GetAsync(groupId);

            if (groupEntity is null) return NotFound();

            return Ok(groupEntity);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditGroup])]
        public async Task<ActionResult<GroupDTO>> CreateAsync([FromBody] GroupDTO groupDTO)
        {
            if (await _groupRepository.GetAsync(groupEntityName: groupDTO.Name) is not null)
            {
                ModelState.AddModelError("Custom Error", "GroupEntity already Exists!");

                return BadRequest(ModelState);
            }

            UserEntity? userEntity = await _userRepository.GetNoTrackingAsync(userEntityId: groupDTO.CuratorId);

            if (userEntity is null) return NotFound("Curator is null");
            if (!UserService.CheckRoleContains(_userRepository, userEntity, Curator)) return BadRequest("User is not a Curator!");
            if (await _groupRepository.CheckExistsAsync(userEntityId: userEntity.Id)) return BadRequest("Curator already have the Group!");

            await _groupRepository.AddAsync(_groupRepository.Create(groupDTO));

            return Created("GroupEntity", groupDTO);
        }

        [HttpPut("{groupId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditGroup])]
        public async Task<IActionResult> UpdateAsync(int groupId, [FromBody] GroupDTO groupDTO)
        {
            if (groupId < 1) return BadRequest();

            GroupEntity? groupEntiyToUpdate = await _groupRepository.GetAsync(groupEntityId: groupId);
            GroupEntity? groupEntityAnother = await _groupRepository.GetAsync(groupEntityName: groupDTO.Name);

            if (groupEntiyToUpdate is null) return NotFound();

            if (groupEntityAnother is not null && groupEntityAnother.Id != groupEntiyToUpdate.Id)
            {
                ModelState.AddModelError("Custom Error", "GroupEntity already Exists!");

                return BadRequest(ModelState);
            }

            UserEntity? userEntity = await _userRepository.GetNoTrackingAsync(userEntityId: groupDTO.CuratorId);

            if (userEntity is null) return NotFound("Curator is null");
            if (!UserService.CheckRoleContains(_userRepository, userEntity, Curator)) return BadRequest("User is not a Curator!");
            if (await _groupRepository.CheckExistsAsync(userEntityId: userEntity.Id) && userEntity.Id != groupEntiyToUpdate.CuratorId) return BadRequest("Curator already have the Group!");

            await _groupRepository.UpdateAsync(groupEntiyToUpdate, groupDTO);

            return NoContent();
        }

        [HttpDelete("{groupId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([EditGroup])]
        public async Task<IActionResult> DeleteAsync(int groupId)
        {
            if (groupId < 1) return BadRequest();

            GroupEntity? groupEntityToRemove = await _groupRepository.GetAsync(groupEntityId: groupId);

            if (groupEntityToRemove is null) return NotFound();

            await _groupRepository.RemoveAsync(groupEntityToRemove);

            return NoContent();
        }
    }
}