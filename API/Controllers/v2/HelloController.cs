using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookHavenAPI.Controllers.v2
{
    [ApiVersion("2.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {

        [HttpGet]
        [MapToApiVersion("2.0")]
        public IActionResult Get() => Ok("Hello from Api version v2 with new features!");
    }
}
