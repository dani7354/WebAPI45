using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Marvin.Cache.Headers;
using WebAPI45.Model;
using WebAPI45.DAL;
using WebAPI45;
using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Identity;
using WebApplication.DataAccess;

namespace WebAPI45
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile(env.ContentRootPath + "/appsettings.json")
                .Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CityDataContext>(options => options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<IdentityDataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });


            var dtoMapper = new DTOMapper().Config.CreateMapper();
            services.AddSingleton(dtoMapper);

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Info() { Title = "Cities API", Version = "v1"}));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDataContext>()
                .AddDefaultTokenProviders();


            services.AddMvc(options =>
            {
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());

            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //Cache
            services.AddHttpCacheHeaders(
                (expirationModelOptions) 
              =>
            {
                expirationModelOptions.MaxAge = 600;
            }, 
            (validationModelOptions) 
                =>
            {
                validationModelOptions.MustRevalidate = true;
            });

            services.AddResponseCaching();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>  c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cities API"));

            app.UseResponseCaching();
            app.UseHttpCacheHeaders();

            app.UseMvc();
        }
    }
}
