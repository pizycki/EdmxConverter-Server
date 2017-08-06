using System.Threading.Tasks;
using Nancy;

namespace WebAPI.Modules
{
    public class ConvertModule : NancyModule
    {
        public ConvertModule()
        {
            Get["/convert", runAsync: true] = async (model, context) => await Task.FromResult("converto");


        }
    }
}