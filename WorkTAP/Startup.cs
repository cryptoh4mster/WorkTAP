using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;   
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkTAP.Models;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using WorkTAP.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging.File;

namespace WorkTAP
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

            var sqlConnectionString = Configuration.GetConnectionString("DataAccessMSSQLProvider");
            services.AddDbContext<PersonsContext>(options => 
                                                    options.UseLazyLoadingProxies().UseSqlServer(sqlConnectionString));
            services.AddControllers();
            services.AddTransient<IWorkTAPService, WorkTAPService>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

        }   


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddFile("Logs/HallOfFame-{Date}.txt");
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // подключаем маршрутизацию на контроллеры
             });

            /*app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {   
                    await context.Response.WriteAsync("Hello World!");
                });
            });*/
        }
    }
}
