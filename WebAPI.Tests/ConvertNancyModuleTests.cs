using Nancy;
using Nancy.Testing;
using Shouldly;
using Xunit;

namespace WebAPI.Tests
{
    public class ConvertNancyModuleTests : IClassFixture<ConvertModuleTests_Fixture>
    {
        // Testing Nancy module
        // https://github.com/NancyFx/Nancy/wiki/Testing-your-application

        private readonly ConvertModuleTests_Fixture _fixture;

        public ConvertNancyModuleTests(ConvertModuleTests_Fixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void returns_200_on_valid_model() =>
            _fixture.Browser.Get("/convert", with => with.HttpRequest())
                            .StatusCode.ShouldBe(HttpStatusCode.OK);


    }

    public class ConvertModuleTests_Fixture
    {
        public Browser Browser { get; }

        public ConvertModuleTests_Fixture()
        {
            var bootstrapper = new DefaultNancyBootstrapper();
            Browser = new Browser(bootstrapper);

        }
    }
}
