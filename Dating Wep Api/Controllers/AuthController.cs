using Dating_Wep_Api.Data.IRepository;
using Dating_Wep_Api.DTO;
using Dating_Wep_Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dating_Wep_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;

        public AuthController(IAuthRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDTO userForRegisterDTO) 
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

    }
}
