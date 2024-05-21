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

namespace api.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController(IConfiguration configuration, AppDbContext db) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly UserRepository _userRepository = new(db);

        [HttpGet]
        public async Task<ActionResult> GetListAsync() => Ok(await _userRepository.GetListAsync());

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
    }
}