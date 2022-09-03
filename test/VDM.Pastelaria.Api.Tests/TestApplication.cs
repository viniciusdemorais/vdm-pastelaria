using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace VDM.Pastelaria.Api.Tests;
internal class TestApplication : WebApplicationFactory<Program>
{
    private readonly string _environment;

    public TestApplication(string environment = "Development") => _environment = environment;

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var mediator = Substitute.For<IMediator>();
        var subAuthenticationSchemeProvider = Substitute.For<IAuthenticationSchemeProvider>();
        subAuthenticationSchemeProvider.GetSchemeAsync(Arg.Any<string>()).Returns(new AuthenticationScheme("Bearer", "Bearer", typeof(MockAuthenticationHandler)));
        builder.ConfigureAppConfiguration((x, _) => x.Configuration["urls"] = "*");
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(_ => Substitute.For<ILogger<string>>());
            services.AddTransient(_ => mediator);
            services.AddTransient(_ => subAuthenticationSchemeProvider);
        });
        builder.UseEnvironment(_environment);
        return base.CreateHost(builder);
    }

    public class MockAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public MockAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
            => Task.FromResult(AuthenticateResult.Success(new(new(new ClaimsIdentity(Array.Empty<Claim>(), "Bearer")), "Bearer")));
    }
}