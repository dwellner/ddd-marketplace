using MarketPlace.CommandHandler;
using MarketPlace.Domain.ClassifiedAd;
using MarketPlace.Domain.Monetization;
using MarketPlace.Infrastructure;
using MarketPlace.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Raven.Client.Documents;

namespace MarketPlace
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var store = new DocumentStore {
                Urls = new [] { "http://localhost:8080"},
                Database = "Marketplace",
                Conventions =
                {
                    FindIdentityProperty = m => m.Name == "_databaseId" 
                }
            };
            store.Initialize();

            services.AddSingleton<ICurrencyLookup, CurrencyLookup>();
            services.AddScoped(c => store.OpenAsyncSession());
            services.AddScoped<IUnitOfWork, RavenDbUnitOfWork>();
            services.AddScoped<IClassifiedAdRepository, ClassifiedAdRavenDbRepository>();
            services.AddScoped<ICommandHandler>(c =>
                new RetryingCommandHandler(
                    new ClassifiedAdsService(
                        c.GetService<IClassifiedAdRepository>(),
                        c.GetService<IUnitOfWork>(),
                        c.GetService<ICurrencyLookup>())));


            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {
                Title="ClassificationAds",
                Version = "v1"
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClassifiedAds V1"));
        }
    }
}
