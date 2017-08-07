using System.Reflection;
using EdmxConverter.WebAPI.Modules;

namespace EdmxConverter.WebAPI.Tests
{
    internal class Referencer
    {
        Assembly WebAPI => Assembly.GetAssembly(typeof(HeartbeatModule));
    }
}
