using System.IO;
using System.Reflection;
using MarketPlace.CommandHandler;
using MarketPlace.Contracts;
using MarketPlace.Domain.ClassifiedAd;
using MarketPlace.Domain.Monetization;
using MarketPlace.EntityStore;
using MarketPlace.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MarketPlace
{
    public static class Program
    {
        static string CurrentDirectory { get; set; }

        static Program() => CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);


        public static void Main(string[] args)
        {
            var configuration = BuildConfiguration(args);
            ConfigureWebHost(configuration).Build().Run();
        }

        public static IWebHostBuilder ConfigureWebHost(IConfiguration configuration) =>
            new WebHostBuilder()
            .UseStartup<Startup>()
            .UseConfiguration(configuration)
            .ConfigureServices(services => services
                .AddSingleton(configuration)
                .AddSingleton<ICurrencyLookup, CurrencyLookup>()
                .AddSingleton<IEntityStore<ClassifiedAd>, EntityStore.EntityStore>()

                .AddSingleton<ICommandHandler>(c =>
                new RetryingCommandHandler(
                    new ClassifiedAdsService(c.GetService< IEntityStore<ClassifiedAd>>(), c.GetService<ICurrencyLookup>()))))
            .UseContentRoot(CurrentDirectory)
            .UseKestrel();


        private static IConfiguration BuildConfiguration(string[] args) =>
            new ConfigurationBuilder().SetBasePath(CurrentDirectory).Build();
    }
}
