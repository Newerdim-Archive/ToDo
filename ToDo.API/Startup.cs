using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDo.API.Data;
using ToDo.API.Helpers;
using ToDo.API.Services;
using ToDo.API.Wrappers;

namespace ToDo.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseNpgsql(Configuration.GetConnectionString("Default"));
            });
            
            services.Configure<TokenSettings>(Configuration.GetSection("TokenSettings"));
            services.AddTransient<ITokenService, TokenService>();

            services.AddHttpContextAccessor();
            services.AddTransient<ICookieService, CookieService>();

            services.AddTransient<IGoogleJsonWebSignatureWrapper, GoogleJsonWebSignatureWrapper>();
            services.Configure<GoogleAuthSettings>(Configuration.GetSection("GoogleAuthSettings"));
            services.AddTransient<IExternalTokenService, GoogleTokenService>();
            
            services.AddAutoMapper(typeof(MapperProfile));
            services.AddTransient<IUserService, UserService>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataContext context)
        {
            if (context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                context.Database.Migrate();
            }
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}