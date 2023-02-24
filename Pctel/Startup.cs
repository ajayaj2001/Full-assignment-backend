using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Logging;
using Services;
using Contracts.Services;
using Contracts.Repositories;
using Entities.DbContexts;
using Repositories;
using CustomExceptionMiddleware;

namespace Pctel
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = Configuration["JwtSecret:Issuer"],
                       ValidAudience = Configuration["JwtSecret:Issuer"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSecret:Key"]))
                   };
               });

            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson(setupAction =>
                 {
                     setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                 });

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            ILogger<ExceptionMiddleware> loggerMiddleware = serviceProvider.GetService<ILogger<ExceptionMiddleware>>();
            services.AddSingleton(typeof(ILogger), loggerMiddleware);

            services.AddSwaggerGen(c =>
       {
           c.EnableAnnotations();
           c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
           {
               Title = "Pctel Api",
               Version = "v1"
           });
       });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddCors(c =>
           {
               c.AddPolicy("AllowOrigin", options =>
               {
                   options.AllowAnyOrigin();
                   options.AllowAnyHeader();
                   options.AllowAnyMethod();
               });
           });

            services.AddDbContext<UserContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowOrigin");

            app.UseAuthentication();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pctel Api");
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
