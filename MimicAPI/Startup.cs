using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MimicAPI.Database;
using MimicAPI.Repositories.Contracts;
using MimicAPI.Repositories;
using AutoMapper;
using MimicAPI.Helpers;

namespace MimicAPI
{
    public class Startup
    { 
        public void ConfigureServices(IServiceCollection services)
        {
            #region AutoMapper Config
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile(new DTOMapperProfile());
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            services.AddDbContext<MimicContext>(opt => {
                opt.UseSqlite(@"Data Source=Database\Mimic.db");
            });

            services.AddMvc();

            //REPOSITORIES
            services.AddScoped<IWordRepository, WordRepository>();
        }

       
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });

            app.UseStatusCodePages();
            app.UseMvc();
        }
    }
}
