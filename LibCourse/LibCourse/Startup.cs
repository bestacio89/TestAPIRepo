using LibCourse.API.DbContexts;
using LibCourse.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace LibCourse.APIAPI
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
           services.AddControllers( setupAction =>
           {
               setupAction.ReturnHttpNotAcceptable = true;
              
           }).AddXmlDataContractSerializerFormatters()
           .ConfigureApiBehaviorOptions(setupAction =>
           {
               setupAction.InvalidModelStateResponseFactory = context =>
               {
                   var problemDetailsFactory = context.HttpContext.RequestServices
                   .GetRequiredService<ProblemDetailsFactory>();
                   var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                       context.HttpContext, context.ModelState);

                   var actionExecutionContext =
                   context as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext; 

                   problemDetails.Detail = "See the errors field for details.";
                   problemDetails.Instance = context.HttpContext.Request.Path;
                   if((context.ModelState.ErrorCount >0)&&(actionExecutionContext?.ActionArguments.Count == context.ActionDescriptor.Parameters.Count))
                   {
                       problemDetails.Type = "https://courselibrary.com/ModelValidationProblem";
                       problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                       problemDetails.Title = "One or more validations occurred";

                       return new UnprocessableEntityObjectResult(problemDetails)
                       {
                           ContentTypes = { "application/json" }
                       };
                   }
                   problemDetails.Status = StatusCodes.Status400BadRequest;
                   problemDetails.Title = "One or more errors on input Occurred";
                   return new BadRequestObjectResult(problemDetails)
                   {
                       ContentTypes = { "application/json" }
                   };
                   
               };

           });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            services.AddScoped<ILibCourseRepository, PersonRepository>();
          

            services.AddDbContext<LibCourseContext>(options =>
            {
                options.UseSqlServer(
                    @"Server=(localdb)\mssqllocaldb;Database=LibCourseDB;Trusted_Connection=True;");
            }); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(
                    appBuilder =>
                    {
                        appBuilder.Run(
                         async context =>
                         {
                             context.Response.StatusCode = 500;
                             await context.Response.WriteAsync("An unexpected fault happened, please try again later");
                         });


                    });
                  
            }
            app.UseRouting();

            app.UsePersonization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
