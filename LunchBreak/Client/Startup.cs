using Blazored.LocalStorage;
using LunchBreak.Client.CodeBehind;
using LunchBreak.Client.Services;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LunchBreak.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBlazoredLocalStorage();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IHttpRequest, HttpRequest>();
            services.AddScoped<IAlertify, Alertify>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
