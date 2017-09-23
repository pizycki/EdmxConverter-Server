using System.Web.Http;

namespace EdmxConv.WebAPI.Controllers
{
    [Route("api/hearbeat")]
    public class HeartbeatController : BaseApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult Beat() => Ok();
    }
}