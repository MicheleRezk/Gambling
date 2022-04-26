using Gambling.Backend.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Gambling.Backend.IntegrationTests
{
    public class TestingWebAppFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
    {
        /*
        protected readonly IPlayerServices PlayerServices;
        protected readonly IBetServices BetServices;
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {

                }
            });
        }
        */
    }
}
