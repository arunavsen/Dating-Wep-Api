using Dating_Wep_Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dating_Wep_Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ValuesController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ValuesController(ApplicationDBContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet(Name = "ListOfValues")]
        public async Task<IActionResult> GetValues()
        {
            var tt = await _context.Values.ToListAsync();
            return Ok(tt);
        }

        [AllowAnonymous]
        [HttpGet("{id}",Name ="OnlySingleValue")]
        public async Task<IActionResult> GetValue(int id)
        {
            if (id is not 0)
            {
                var value = await _context.Values.FirstOrDefaultAsync(x => x.Id == id);

                if (value == null) 
                { 
                    return BadRequest(); 
                }
                return Ok(value);
            }
            return BadRequest();
        }
    }
}
