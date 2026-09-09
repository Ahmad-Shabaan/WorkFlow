using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookHavenAPI.Controllers.v3
{
    [ApiVersion("3.0")]

    //v{version:apiVersion} route constraint that binds the URL version segment to the API version system.
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //This is the value that DocInclusionPredicate checks to decide which Swagger doc this controller's endpoints belong to.
    //[ApiExplorerSettings(GroupName = "v3.0")] // framework auto-assign the group name based on GroupNameFormat
    public class HelloController : ControllerBase
    {
        [HttpGet]
        [MapToApiVersion("3.0")]
        public IActionResult Get() => Ok("Hello from Api version v3 with more new features!");
    }
}
