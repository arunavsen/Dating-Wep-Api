using Dating_Wep_Api.Data;
using Microsoft.AspNetCore.Mvc;

namespace Dating_Wep_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValuesController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ValuesController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "ListOfValues")]
        public IActionResult GetValues()
        {
            return Ok(_context.Values.ToList());
        }

        [HttpGet("{id}",Name ="OnlySingleValue")]
        public IActionResult GetValue(int id)
        {
            if (id is not 0)
            {
                var value = _context.Values.FirstOrDefault(x => x.Id == id);

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
