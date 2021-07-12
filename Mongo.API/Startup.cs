using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Mongo.Database;
using Mongo.Database.Interfaces;
using Mongo.Services;
using Mongo.Services.Interfaces;
using Mongo.Settings;
using AutoMapper;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Mongo.API.Middleware;

namespace Mongo.API
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
            services.AddControllers();
            services.AddCors();

            services.AddSingleton<IUsersContext, UsersContext>();
            services.Configure<ApplicationSettings>(Configuration);
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<IJWTServices, JWTServices>();
            services.AddTransient<IEmailServices, EmailServices>();
            services.AddAutoMapper(typeof(Startup));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mongo.API", Version = "v1" });
            });


            //Configure JWT token
            var jwtSettings = Configuration.GetSection("AppSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["JWtSecret"]);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer("Bearer", x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = "Issuer",
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.Zero,
                    };
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //Enable swagger api
            // if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mongo.API v1"));
            }

            //app.Run(ExceptionMiddleware);
            //app.UseMiddleware<ExceptionMiddleware>();
            app.ConfigureCustomExceptionMiddleware();


            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        // Configu exception middle ware 
        //private Task ExceptionMiddleware(HttpContext context)
        //{
        //    return context.Response.WriteAsync("Hello World! + Exception ");
        //}

        
    }
}
