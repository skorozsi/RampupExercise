using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Users.Api.Models;
using Users.Api.Providers;
using Users.Api.Repositories;
using Users.Api.Services;

namespace Users.Api
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
            services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("usersdb"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //services.AddDbContext<UserContext>(opt => opt.UseInMemoryDatabase("UsersApi"));

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Users API",
                    Version = "v1",
                    Description = "CRUD services for Users. User creation is logged using RabbitMQ to a consumer application.",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Sandor Korozsi",
                        Email = string.Empty,
                        Url = "https://twitter.com/skorozsi"
                    },
                    License = new License
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license"
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });


            services.AddSingleton<IUserCreationLogService, UserCreationLogService>();
            services.AddScoped<IUserRepository, UserDBRepository>();
            services.AddSingleton<IDBProvider, InMemoryDbProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Users API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var context = serviceProvider.GetService<ApiContext>();
            AddTestData(context);

            app.UseMvc();
        }

        private static void AddTestData(ApiContext context)
        {
            var luke = new User
            {
                Id = 1,
                Name = "Luke Skywalker",
                DoB= DateTime.Today.AddYears(-20)
            };
            var obiOne = new User
            {
                Id = 2,
                Name = "Obivan Kenobi",
                DoB= DateTime.Today.AddYears(-80)
            };
            var darthVader = new User
            {
                Id = 3,
                Name = "Darth Vader",
                DoB= DateTime.Today.AddYears(-60)
            };

            context.Users.Add(obiOne);
            context.Users.Add(luke);
            context.Users.Add(darthVader);

            context.SaveChanges();
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

}
