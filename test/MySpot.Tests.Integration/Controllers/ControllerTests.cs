using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MySpot.Application.DTO;
using MySpot.Application.Security;
using MySpot.Infrastructure.Auth;
using MySpot.Infrastructure.Services;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Xunit;

namespace MySpot.Tests.Integration.Controllers
{
    [Collection("api")]
    public abstract class ControllerTests : IClassFixture<OptionsProvider>
    {
        protected HttpClient Client { get; }
        private readonly IAuthenticator _authenticator;

        protected JwtDto Authorize(Guid userId, string role)
        {
            var jwt = _authenticator.CreateToken(userId, role);
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.AccessToken);
            return jwt;
        }

        public ControllerTests(OptionsProvider optionsProvider) 
        {
            var authOptions = optionsProvider.Get<AuthOptions>("auth");
            _authenticator = new Authenticator(new OptionsWrapper<AuthOptions>(authOptions), new Clock());
            var app = new MySpotTestApp(ConfigureServices);
            Client = app.Client;
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
        }
    }
}
