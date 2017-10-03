using System.Web.Http;
using System.Web.Http.Cors;

namespace EdmxConv.WebAPI.Controllers
{
    [RoutePrefix("api/heartbeat")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HeartbeatController : BaseApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult Beat() => Ok();
    }
}