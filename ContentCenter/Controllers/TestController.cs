using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ContentCenter.Controllers
{
    [Route("api/v1/[controller]")]
    public class TestController : Controller
    {
        // GET api/v1/test
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok($"{id}");
        }       
    }
}
