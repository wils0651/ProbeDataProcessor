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
    static async Task Main(string[] args)
    {
        string environment = Environment.GetEnvironmentVariable("APP_ENV") ?? "Development";

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

        IConfiguration configuration = builder.Build();

        var services = new ServiceCollection();

        // Jobs
        services.AddSingleton<ProcessTemperatureDataJob>();
        services.AddSingleton<DeleteGarageDistanceSensorDataJob>();

        // Services
        services.AddScoped<IProcessTemperatureDataService, ProcessTemperatureDataService>();
        services.AddScoped<IGarageDistanceDataService, GarageDistanceDataService>();

        // Repositories
        services.AddScoped<IProbeDataRepository, ProbeDataRepository>();
        services.AddScoped<IProbeRepository, ProbeRepository>();
        services.AddScoped<ITemperatureStatisticRepository, TemperatureStatisticRepository>();
        services.AddScoped<IGarageDistanceSensorDataRepository, GarageDistanceSensorDataRepository>();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        var serviceProvider = services.BuildServiceProvider();

        // Get Job
        var job = GetJob(args, serviceProvider);

        // Run the job
        await job.RunAsync();
    }

    /// <summary>
    /// Get the job to run.
    /// </summary>
    /// <param name="jobType"></param>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    /// <exception cref="InvalidDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private static IJob GetJob(string[] args, IServiceProvider serviceProvider)
    {
        var jobName = args == null || string.IsNullOrEmpty(args[0])
            ? string.Empty
            : args[0];

        IJob? job;
        switch (jobName.ToLower())
        {
            case "processtemperaturedata":
                {
                    job = serviceProvider.GetService<ProcessTemperatureDataJob>();
                    break;
                }
            case "deletegaragedata":
                {
                    job = serviceProvider.GetService<DeleteGarageDistanceSensorDataJob>();
                    break;
                }
            default:
                {
                    job = serviceProvider.GetService<ProcessTemperatureDataJob>();
                    break;
                }
        }

        if (job == null)
        {
            throw new InvalidOperationException("Job is null");
        }

        return job;
    }
}
