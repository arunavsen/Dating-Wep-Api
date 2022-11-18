using Dating_Wep_Api.IRepo;
using Microsoft.AspNetCore.Mvc;

namespace Dating_Wep_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnimalController : Controller
    {
        private readonly IAnimal _animal;

        public AnimalController(IAnimal animal)
        {
            _animal = animal;
        }

        [HttpGet]
        public IActionResult GetAllAnimals() 
        {
            return Ok(_animal.GetAllAnimal());
        }
    }
}
