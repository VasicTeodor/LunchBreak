using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System.Linq;
using AutoMapper;
using System.Text;
using LunchBreak.Helpers;
using LunchBreak.Infrastructure.DatabaseSettings;
using LunchBreak.Infrastructure.Interfaces;
using LunchBreak.Infrastructure.Repository;
using LunchBreak.Server.ServicesSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace LunchBreak.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private TokenValidationParameters GetTokenValidationParameters()
        {
            var secret = Configuration.GetSection("Secret").Value;
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            return tokenValidationParameters;
        }
        private static void ConfigureAuthorization(AuthorizationOptions options)
        {
            var user = HelperAuth.UserClaim();
            var editor = HelperAuth.EditorClaim();
            var admin = HelperAuth.AdminClaim();
            options.AddPolicy(HelperAuth.Constants.Policy.User, policy =>
            {
                policy.RequireAssertion(context =>
                {
                    var ret = context.User.HasClaim(c => c.Type == user);
                    return ret;
                });
            });
            options.AddPolicy(HelperAuth.Constants.Policy.Editor, policy =>
            {
                policy.RequireAssertion(context =>
                {
                    var ret = context.User.HasClaim(c => c.Type == user) && context.User.HasClaim(c => c.Type == editor);
                    return ret;
                });
            });
            options.AddPolicy(HelperAuth.Constants.Policy.Admin, policy =>
            {
                policy.RequireAssertion(context =>
                {
                    var ret = context.User.HasClaim(c => c.Type == user) && context.User.HasClaim(c => c.Type == admin);
                    return ret;
                });
            });
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DatabaseSettings>(
                Configuration.GetSection(nameof(DatabaseSettings)));

            services.AddScoped<IDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = GetTokenValidationParameters();
                });

            services.AddAuthorization(ConfigureAuthorization);

            // Repositories
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILunchRepository, LunchRepository>();
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();

            // Register Mapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddMvc().AddNewtonsoftJson();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBlazorDebugging();
            }

            //app.ConfigureCustomExceptionMiddleware();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseClientSideBlazorFiles<Client.Startup>();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapFallbackToClientSideBlazor<Client.Startup>("index.html");
            });
        }
    }
}
