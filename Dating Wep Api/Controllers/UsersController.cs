using AutoMapper;
using Dating_Wep_Api.Data.IRepository;
using Dating_Wep_Api.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dating_Wep_Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _datingRepo;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository datingRepo, IMapper mapper)
        {
            this._datingRepo = datingRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _datingRepo.GetUsers();
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDTO>>(users);
            return Ok(usersToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _datingRepo.GetUser(id);
            var userToReturn = _mapper.Map<UserForDetailsDTO>(user);
            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDTO userForUpdateDTO)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var userFromRepo = await _datingRepo.GetUser(id);

            _mapper.Map(userForUpdateDTO, userFromRepo);

            if (await _datingRepo.SaveAll())
            {
                return NoContent();
            }

            throw new Exception($"Updating user {id} failed on save");
        }
    }
}
