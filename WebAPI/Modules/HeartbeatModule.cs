using Nancy;

namespace WebAPI.Modules
{
    public class HeartbeatModule : NancyModule
    {
        public HeartbeatModule()
        {
            Get["/heartbeat"] = _ => "Ok";
        }
    }
}