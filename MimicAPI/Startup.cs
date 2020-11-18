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
using MimicAPI.V1.Repositories.Contracts;
using MimicAPI.V1.Repositories;
using AutoMapper;
using MimicAPI.Helpers;
using MimicAPI.Helpers.Swagger;
using Swashbuckle.AspNetCore.Swagger;

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
            services.AddApiVersioning(cfg =>{
                cfg.ReportApiVersions = true;
                cfg.AssumeDefaultVersionWhenUnspecified = true;
                cfg.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion (1, 0);
            });

            services.AddSwaggerGen(cfg => {
                cfg.ResolveConflictingActions(apiDescription => apiDescription.First());
                cfg.SwaggerDoc("v2.0", new Info() { 
                    Title = "MimicAPI - V2.0", 
                    Version = "v2.0"});

                cfg.SwaggerDoc("v1.1", new Info() { 
                    Title = "MimicAPI - V1.1", 
                    Version = "v1.1"});

                cfg.SwaggerDoc("v1.0", new Info() { 
                    Title = "MimicAPI - V1.0", 
                    Version = "v1.0"});

                cfg.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var actionApiVersionModel = apiDesc.ActionDescriptor?.GetApiVersion();
                    // would mean this action is unversioned and should be included everywhere
                    if (actionApiVersionModel == null)
                    {
                        return true;
                    }
                    if (actionApiVersionModel.DeclaredApiVersions.Any())
                    {
                        return actionApiVersionModel.DeclaredApiVersions.Any(v => $"v{v.ToString()}" == docName);
                    }
                    return actionApiVersionModel.ImplementedApiVersions.Any(v => $"v{v.ToString()}" == docName);
                });

                cfg.OperationFilter<ApiVersionOperationFilter>();
            });

            //REPOSITORIES
            services.AddScoped<IWordRepository, WordRepository>();
        }

       
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();
            
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(cfg => {
                cfg.SwaggerEndpoint("/swagger/v2.0/swagger.json", "MimicAPI - V2.0");
                cfg.SwaggerEndpoint("/swagger/v1.1/swagger.json", "MimicAPI - V1.1");
                cfg.SwaggerEndpoint("/swagger/v1.0/swagger.json", "MimicAPI - V1.0");
                cfg.RoutePrefix = String.Empty;
            });

        }
    }
}
