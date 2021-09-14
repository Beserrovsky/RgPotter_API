using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RG_Potter_API.DB;
using RG_Potter_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RG_Potter_API
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

            ConfigureCors(services);

            ConfigureControllers(services);

            ConfigureEF(services);

            ConfigureConfiguration(services);

            ConfigureHash(services);

            ConfigureJWT(services);
        }

        private void ConfigureCors(IServiceCollection services)
        {
            var allowedUrls = Configuration.GetSection("AllowedCorsUrls")
                                  ?.Get<List<string>>()
                                  ?.ToArray();

            services.AddCors(options => options.AddPolicy("ApiCorsPolicy",
                builder => { builder.WithOrigins(allowedUrls).AllowAnyMethod().AllowAnyHeader(); }));
        }

        private void ConfigureControllers(IServiceCollection services) 
        {
            services.AddControllers().AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        private void ConfigureEF(IServiceCollection services) 
        {
            services.AddDbContext<PotterContext>(options =>
                options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));
        }

        private void ConfigureConfiguration(IServiceCollection services) 
        {
            services.AddSingleton(Configuration);
        }

        private void ConfigureHash(IServiceCollection services)
        {
            services.AddScoped<IPasswordHash, Sha256PasswordHash>();
        }

        private void ConfigureJWT(IServiceCollection services) 
        {
            var key = Encoding.ASCII.GetBytes(Configuration["Jwt:Key"]);

            var issuer = Configuration["Jwt:Issuer"];

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
