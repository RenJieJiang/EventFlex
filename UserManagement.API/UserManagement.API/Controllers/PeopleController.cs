using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.API.Constants;

namespace UserManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PeopleController : ControllerBase
    {

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Post()
        {
            return Ok();
        }
    }
}
