using BookStore.API.Configurations;
using BookStore.API.GraphQL;
using BookStore.Core.Repositories;
using BookStore.Infrastructure.Data;
using BookStore.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.GraphQL
{
    public class Startup
    {
        private readonly ApiConfiguration _apiConfiguration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _apiConfiguration = configuration.Get<ApiConfiguration>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // configurations
            services.AddSingleton(_apiConfiguration.MongoDbConfiguration);

            // repositories
            services.AddSingleton<IBookContext, BookContext>();
            services.AddScoped<IBookRepository, BookRepository>();

            // CORS configuration
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });

            // GraphQL
            services.AddGraphQLServer()
                .AddQueryType<Query>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable CORS before routing
            app.UseCors("AllowAngularApp");

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}
