using Dating_Wep_Api.Data.IRepository;
using Dating_Wep_Api.DTO;
using Dating_Wep_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dating_Wep_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDTO userForRegisterDTO) 
        {
            var username = userForRegisterDTO.username;

            username = username.ToLower();

            if (await _repo.UserExist(username))
            {
                return BadRequest("Username already exist");
            }

            var userToCreate = new User
            {
                Username = username
            };

            var createdUser = await _repo.RegisterUser(userToCreate, userForRegisterDTO.password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDTO)
        {
            var userFromRepo = await _repo.Login(userForLoginDTO.Username, userForLoginDTO.Password);

            if (userFromRepo == null)
            {
                return Unauthorized();
            }

            #region How to create token after login
            
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Appsettings:Token").Value));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.Now.AddMinutes(120),
                        SigningCredentials = signingCredentials
                        };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            #endregion

            return Ok(tokenString);
        }

    }
}
