using Microsoft.AspNetCore.Mvc;

namespace EdmxConv.WebAPI.Controllers
{
    [Route("api/hearbeat")]
    public class HeartbeatController : ApiController
    {
        [HttpGet, Route("")]
        public IActionResult Beat() => Ok();
    }
}