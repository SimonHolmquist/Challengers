using Challengers.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Challengers.AuthTests;

public class CustomAuthWebApplicationFactoryForAuth : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            Environment.SetEnvironmentVariable("ENABLE_AUTH", "true");
        });
    }
}
