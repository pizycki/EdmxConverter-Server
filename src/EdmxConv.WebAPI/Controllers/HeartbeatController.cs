using System.Web.Http;

namespace EdmxConv.WebAPI.Controllers
{
    [RoutePrefix("api/heartbeat")]
    public class HeartbeatController : BaseApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult Beat() => Ok();
    }
}