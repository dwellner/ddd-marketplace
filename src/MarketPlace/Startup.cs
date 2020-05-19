using System;
using System.Collections.Generic;
using System.Linq;
using EventStore.ClientAPI;
using MarketPlace.ClassifiedAd;
using MarketPlace.Domain.Monetization;
using MarketPlace.Projections;
using MarketPlace.Service;
using MarketPlace.UserProfile;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static MarketPlace.Projections.ReadModels;

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
            var esConnection = EventStoreConnection.Create(
                Configuration["eventStore:connectionString"],
                ConnectionSettings.Create().KeepReconnecting(),
                Environment.ApplicationName);

            var store = new EventStoreAggregateStore(esConnection);

            services.AddSingleton(esConnection);
            services.AddSingleton<IAggregateStore>(store);

            services.AddSingleton<ICurrencyLookup, CurrencyLookup>();
            services.AddSingleton<IFailoverPolicyProvider, FailoverPolicyFactory>();
            services.AddScoped<ClassifiedAdsService>();
            services.AddScoped<UserProfileService>();

            var ads = new List<ClassifiedAdDetails>();
            var users = new List<UserDetails>();
            services.AddSingleton<IEnumerable<ClassifiedAdDetails>>(ads);
            var subscription = new ProjectionsManager(esConnection, new List<IProjection> {
                new ClassifiedAdDetailsProjection(ads, id => users.FirstOrDefault(u => u.Id == id)?.Displayname),
                new UserDetailsProjection(users)
            });
            services.AddSingleton<IHostedService>(new EventStoreService(esConnection, subscription));

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
