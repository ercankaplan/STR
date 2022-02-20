using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Serilog;
using STR.Data.Models.Ef.EfContext;
using STR.Reporting.Interfaces;
using STR.Reporting.Services;
using System;
using System.IO;

namespace STR.Reporting
{
    class Program
    {
        public static IConfigurationRoot configuration;
        static void Main(string[] args)
        {
            
            //string connectionString = "User ID=postgres;password=1234qqqQ;Host=localhost;port=5432;Database=STRDB";

            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            serviceCollection.AddSingleton<ReportsService>();

            serviceCollection.AddDbContext<STRDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("ConnStr"), b => b.MigrationsAssembly("STR.Data"));
            });


            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            var reportsService = serviceProvider.GetService<ReportsService>();


            Log.Information("Creating Cunsumer");

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            QueueConsumer.Consume(channel, reportsService);


        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Add logging
            serviceCollection.AddSingleton(LoggerFactory.Create(builder =>
            {
                builder.AddSerilog(dispose: true);
            }));

            serviceCollection.AddLogging();

            // Build configuration
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton<IConfigurationRoot>(configuration);

            // Add app
            serviceCollection.AddTransient<Program>();
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //   Host.CreateDefaultBuilder(args)
        //       .ConfigureServices(services =>
        //       {

        //           services.AddTransient<ReportsService>();
        //       });


    }
}
