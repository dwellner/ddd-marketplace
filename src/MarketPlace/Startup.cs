using System;
using MarketPlace.ClassifiedAd;
using MarketPlace.Domain.ClassifiedAd;
using MarketPlace.Domain.Monetization;
using MarketPlace.Domain.UserProfile;
using MarketPlace.Service;
using MarketPlace.UserProfile;
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
            services.AddSingleton<IFailoverPolicyProvider, FailoverPolicyFactory>();
            services.AddScoped(c => store.OpenAsyncSession());
            services.AddScoped<IUnitOfWork, RavenDbUnitOfWork>();
            services.AddScoped<IClassifiedAdRepository, ClassifiedAdRepository>();
            services.AddScoped<ClassifiedAdsService>();

            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            services.AddScoped<UserProfileService>();

            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("classified-ads-v1", new Microsoft.OpenApi.Models.OpenApiInfo {
                    Title = "ClassificationAds", Version = "v1", });
                c.SwaggerDoc("userprofiles-v1", new Microsoft.OpenApi.Models.OpenApiInfo {
                    Title = "UserProfiles", Version = "v1" });

                c.DocInclusionPredicate((docName, apiDesc) => {
                    Console.WriteLine($"{docName}: {apiDesc.RelativePath}");
                    return docName switch
                    {
                        "classified-ads-v1" => apiDesc.RelativePath.StartsWith("v1/ad"),
                        "userprofiles-v1" => apiDesc.RelativePath.StartsWith("v1/userprofile"),
                        _ => true
                    };
                });
            });
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
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/classified-ads-v1/swagger.json", "ClassifiedAds V1");
                c.SwaggerEndpoint("/swagger/userprofiles-v1/swagger.json", "UserProfiles V1");
            });
        }
    }
}
