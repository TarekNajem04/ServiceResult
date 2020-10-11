using System;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceResult.Domain.DataTransferObjects;
using ServiceResult.Domain.Entities;

namespace ServiceResult.ExceptionPattern.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper
                              (
                                      configAction =>
                                      {
                                          configAction
                                             .CreateMap<Login, LoginDto>()
                                             .ReverseMap()
                                             .ForMember((dest) => dest.Id, _ => Guid.NewGuid());
                                          // To Avoid Recursive Mapping
                                          // configAction.ForAllMaps((map, exp) => exp.MaxDepth(2));
                                      }, Assembly.GetExecutingAssembly()
                              );

            services.AddTransient<ILoginService, LoginService>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}