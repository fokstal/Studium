using api.Data;
using api.Model;
using api.Model.DTO;
using api.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RegisterUserDTO>> RegisterAsync([FromBody] RegisterUserDTO userDTO)
        {
            using (AppDbContext db = new())
            {
                if (await db.User.FirstOrDefaultAsync(userDb => userDb.Login.ToLower() == userDTO.Login.ToLower()) is not null)
                {
                    ModelState.AddModelError("Custom", "User already Exists!");

                    return BadRequest(ModelState);
                }

                string passwordHash = StringHasher.Generate(userDTO.Password);

                await db.User.AddAsync(new()
                {
                    Login = userDTO.Login,
                    Email = userDTO.Email,
                    PasswordHash = passwordHash,
                    DateCreated = DateTime.Now,
                });

                await db.SaveChangesAsync();
            }

            return Created("User", userDTO);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> LoginAsync([FromBody] LoginUserDTO userDTO)
        {
            using (AppDbContext db = new())
            {
                User? user = await db.User.FirstOrDefaultAsync(userDb => userDb.Login.ToLower() == userDTO.Login.ToLower());

                if (user is null) return NotFound();

                if (StringHasher.Verify(userDTO.Password, user.PasswordHash) is false) return BadRequest();

                HttpContext.Response.Cookies.Append("Cookie", JwtProvider.GenerateToken(user));

                return Ok();
            }
        }
    }
}