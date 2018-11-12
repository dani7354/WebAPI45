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
using WebAPI45.Model;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;

namespace WebAPI45
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
             //   .AddJsonFile(env.ContentRootPath + "/connectionStrings.json")
                .Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CityDataContext>(options => options.UseInMemoryDatabase("webAPI"));

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CityDTOnoAttractions, City>();
                cfg.CreateMap<CityDTOwithAttractions, City>();
                cfg.CreateMap<TouristAttractionDTO, TouristAttraction>();
            });
            var dtoMapper = mapperConfig.CreateMapper();
            services.AddSingleton(dtoMapper);

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Info() { Title = "Cities API", Version = "v1" }));

            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddXmlSerializerFormatters();
            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cities API"));

            app.UseMvc();
        }
    }
}
