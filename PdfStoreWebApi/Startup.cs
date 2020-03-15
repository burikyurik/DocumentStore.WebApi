using System;
using System.Threading.Tasks;
using CircuitBreaker.Net;
using DocumentStore.Application;
using DocumentStore.Application.Command;
using DocumentStore.Application.CommandHandlers;
using DocumentStore.Application.Query;
using DocumentStore.Application.QueryHandlers;
using DocumentStore.Application.Validation;
using DocumentStore.Infrastructure.Configuration;
using DocumentStore.Infrastructure.Models;
using DocumentStore.WebApi.ActionFilters;
using DocumentStore.WebApi.Configuration;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DocumentStore.WebApi
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
            services.AddControllers(options => options.Filters.Add(typeof(ValidateModelStateAttribute)));
            services.AddSwaggerDocumentation();

            services.AddScoped<ISortHelper<Document>, SortHelper<Document>>();
            services.AddMediatR(typeof(GetDocumentsQuery));
            services.AddScoped<IValidator<UploadDocumentCommand>, UploadDocumentCommandValidator>();
            services.AddSingleton<ICircuitBreaker>(provider => new CircuitBreaker.Net.CircuitBreaker(
                TaskScheduler.Default,
                maxFailures: 3,
                invocationTimeout: TimeSpan.FromSeconds(30),
                circuitResetTimeout: TimeSpan.FromSeconds(60)));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestHandlerBehaviorWithCircuitBreaker<,>));

            var azureStorageConnectionString = Configuration["AzureStorageConnectionString"];
            services.AddAzureStorage(azureStorageConnectionString);

            services.AddCosmosDb(Configuration["CosmosEndPointUrl"], Configuration["CosmosAuthorizationKey"], "DocumentStore");

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
            app.UseExceptionHandlingMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerDocumentation();
        }
    }
}
