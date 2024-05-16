using api.Data;
using api.Models;
using api.Model.DTO;
using api.Services;
using api.Repositories.Data;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController(IConfiguration configuration, AppDbContext db) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly UserRepository _userService = new(db);

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RegisterUserDTO>> RegisterAsync([FromBody] RegisterUserDTO userDTO)
        {
            if (await _userService.GetAsync(userDTO.Login) is not null)
            {
                ModelState.AddModelError("Custom", "User already Exists!");

                return BadRequest(ModelState);
            }

            string passwordHash = StringHasher.Generate(userDTO.Password);

            await _userService.AddAsync(new()
            {
                Login = userDTO.Login,
                Email = userDTO.Email,
                PasswordHash = passwordHash,
                DateCreated = DateTime.Now,
            });

            return Created("User", userDTO);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserDTO userDTO)
        {
            UserEntity? user = await _userService.GetAsync(userDTO.Login);

            if (user is null) return NotFound();

            if (StringHasher.Verify(userDTO.Password, user.PasswordHash) is false) return BadRequest();

            HttpContext.Response.Cookies.Append("Cookie", new JwtProvider(_configuration).GenerateToken(user));

            return Ok();
        }
    }
}