using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EdmxConverter.WebAPI.Controllers
{
    [Route("api/heartbeat")]
    public class HeartbeatController : Controller
    {
        [HttpGet, Route("")]
        public IActionResult Get() => Ok();
    }
}
