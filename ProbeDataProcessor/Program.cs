using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProbeDataProcessor;
using ProbeDataProcessor.Contracts;
using ProbeDataProcessor.Jobs;
using ProbeDataProcessor.Repositories;
using ProbeDataProcessor.Services;


class Program
{
    static async Task Main()
    {
        string environment = Environment.GetEnvironmentVariable("APP_ENV") ?? "Development";

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

        IConfiguration configuration = builder.Build();

        var services = new ServiceCollection();

        services.AddSingleton<ProcessTemperatureDataJob>();

        services.AddScoped<IProcessTemperatureDataService, ProcessTemperatureDataService>();

        services.AddScoped<IProbeDataRepository, ProbeDataRepository>();
        services.AddScoped<IProbeRepository, ProbeRepository>();
        services.AddScoped<ITemperatureStatisticRepository, TemperatureStatisticRepository>();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        var serviceProvider = services.BuildServiceProvider();

        var processTemperatureDataJob = serviceProvider.GetService<ProcessTemperatureDataJob>();

        await processTemperatureDataJob.RunAsync();
    }
}