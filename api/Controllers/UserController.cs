using api.Data;
using api.Models;
using api.Model.DTO;
using api.Services;
using api.Repositories.Data;
using Microsoft.AspNetCore.Mvc;
using api.Extensions;
using api.Helpers.Constants;

using static api.Helpers.Enums.RoleEnum;
using static api.Helpers.Enums.PermissionEnum;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace api.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController(IConfiguration configuration, AppDbContext db) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly UserRepository _userRepository = new(db);
        private readonly StudentRepository _studentRepository = new(db);

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequireRoles([Admin, Secretar])]
        [RequirePermissions([ViewUserList])]
        public async Task<ActionResult> GetListAsync() => Ok(await _userRepository.GetListAsync());

        [HttpGet("session")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserEntity>> GetSessionAsync()
        {
            string? token = HttpContext.Request.Cookies[CookieNames.USER_TOKEN];

            if (token is null) return Unauthorized();

            JwtSecurityTokenHandler handler = new();

            JwtSecurityToken jwtToken = handler.ReadJwtToken(token);

            Claim userIdClaim = jwtToken.Claims.First(claim => claim.Type == CustomClaims.USER_ID);

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out Guid userId)) return Unauthorized();

            return Ok(await _userRepository.GetAsync(userId));
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequireRoles([Admin, Secretar])]
        [RequirePermissions([RegisterUser])]
        public async Task<ActionResult<RegisterUserDTO>> RegisterAsync([FromBody] RegisterUserDTO userDTO)
        {
            if (await _userRepository.GetAsync(userDTO.Login) is not null)
            {
                ModelState.AddModelError("Custom", "User already Exists!");

                return BadRequest(ModelState);
            }

            userDTO.Id = Guid.NewGuid();

            await _userRepository.AddAsync(_userRepository.Create(userDTO));

            return Created("User", userDTO);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserDTO userDTO)
        {
            UserEntity? user = await _userRepository.GetAsync(userDTO.Login);

            if (user is null) return NotFound();

            if (StringHasher.Verify(userDTO.Password, user.PasswordHash) is false) return BadRequest("Password is not valid!");

            HttpContext.Response.Cookies.Append(CookieNames.USER_TOKEN, new JwtProvider(_configuration).GenerateToken(user));

            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequirePermissions([RegisterUser])]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            UserEntity? userToRemove = await _userRepository.GetNoTrackingAsync(id);

            if (userToRemove is null) return NotFound();

            if (UserService.CheckRoleContains(_userRepository, userToRemove, Student))
            {
                StudentEntity studentToRemove = await _studentRepository.GetAsync(userToRemove.Id) ?? throw new Exception("Student on User is null!");

                await _studentRepository.RemoveAsync(studentToRemove);
            }

            await _userRepository.RemoveAsync(userToRemove);

            return NoContent();
        }
    }
}