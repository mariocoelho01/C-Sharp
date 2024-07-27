using Blog.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Blog.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet("")]
        //[ApiKey] only for when passing KeyApi in url
        public IActionResult Get()
            => Ok(new { Active = "on" });
    }
}
