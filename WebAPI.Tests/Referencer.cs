using System.Reflection;
using WebAPI.Modules;

namespace WebAPI.Tests
{
    internal class Referencer
    {
        Assembly WebAPI => Assembly.GetAssembly(typeof(HeartbeatModule));
    }
}
