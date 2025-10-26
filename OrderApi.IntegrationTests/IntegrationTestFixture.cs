using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace OrderApi.IntegrationTests;

public class IntegrationTestFixture : IAsyncLifetime
{
    private WebApplicationFactory<Program>? _webApplicationFactory;
    public WebApplicationFactory<Program> WebApplicationFactory => _webApplicationFactory
                                                                    ?? throw new InvalidOperationException("WebApplicationFactory has not been initialized.");

    public async Task InitializeAsync()
    {
        await Task.WhenAll();
        var webApplicationFactory = CreateWebApplicationFactory();
        _webApplicationFactory = webApplicationFactory;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    protected WebApplicationFactory<Program> CreateWebApplicationFactory()
    {
        return new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    // { "ConnectionStrings:MongoDBConnection", _mongoDbContainer.GetConnectionStringForDatabase("campaigns") },
                    // { "ConnectionStrings:StudioStorage", _azuriteContainer.GetConnectionString() },
                })
                .AddEnvironmentVariables()
                .Build();

            builder.UseConfiguration(config);

            builder.ConfigureServices(services =>
            {

            });

            builder.ConfigureTestServices(services =>
            {

            });

            builder.UseTestServer();
        });
    }
}