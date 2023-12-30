using BusinessLayer.Interface;
using BusinessLayer.Servises;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using RepositoryLayer.context;
using RepositoryLayer.@interface;
using RepositoryLayer.servises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotes
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
            services.AddDbContext<FundooContext>(a => a.UseSqlServer(Configuration["ConnectionStrings:ConFundooDB"]));

            services.AddTransient<IUserBusiness, UserBusiness>();
            services.AddTransient<IuserRepo, userRepo>();

            services.AddTransient<INoteRepo, NoteRepo>();

            services.AddTransient<INotesBusiness, NotesBusiness>();

            services.AddTransient<ILabelRepo, LabelRepo>();
            services.AddTransient<ILabelsBusiness, LabelsBusiness>();
            services.AddTransient<ICollaboratorBusiness, CollaboratorBusiness>();
            services.AddTransient<ICollaboratorsRepo, CollaboratorsRepo>();
           

            services.AddSwaggerGen(option =>
                    {
                      option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                       option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                   {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "Jwt",
                    Scheme = "Bearer"
                    });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                  {
                    {
                        new OpenApiSecurityScheme
                      {
                       Reference = new OpenApiReference
                 {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                 }
                      },
                       new string[]{}
                    }
                   });
        });

           services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddStackExchangeRedisCache(options => { options.Configuration = Configuration["RedisCacheUrl"]; });


            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddNLog("NLog.config");
                loggingBuilder.AddConsole();

            });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust as needed
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            


            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config=>
                {
                    config.UseHealthCheck(provider);
                    config.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                }));
            });

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var Key = Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Key)
                };
            });

            //services.AddSingleton<IJWTManagerRepository, JWTManagerRepository>();

            services.AddControllers();

            services.AddMassTransitHostedService();
            services.AddScoped<IBus>(provider => provider.GetRequiredService<IBusControl>());

        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSession();

            // This middleware serves the Swagger documentation UI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FundooContext API V1");
            });

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });




        }
    }
}
