using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProbeDataProcessor;
using ProbeDataProcessor.Contracts;
using ProbeDataProcessor.Enums;
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

        services.AddSingleton<ProcessTemperatureDataJob>();

        services.AddScoped<IProcessTemperatureDataService, ProcessTemperatureDataService>();

        services.AddScoped<IProbeDataRepository, ProbeDataRepository>();
        services.AddScoped<IProbeRepository, ProbeRepository>();
        services.AddScoped<ITemperatureStatisticRepository, TemperatureStatisticRepository>();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        var serviceProvider = services.BuildServiceProvider();

        // Chose the job to run based on command line args

        // Get comand line argument
        var jobName = args[0];

        var jobType = GetJobType(jobName);

        // Run the job
        switch (jobType)
        {
            case JobType.ProcessTemperatureData:
                {
                    var processTemperatureDataJob = serviceProvider.GetService<ProcessTemperatureDataJob>();

                    await processTemperatureDataJob.RunAsync();
                    break;
                }
            default:
                {
                    throw new InvalidDataException($"Invalid JobType: {jobType}");
                }
        }
    }

    private static JobType GetJobType(string jobName)
    {
        return jobName.ToLower() switch
        {
            "processtemperaturedata" => JobType.ProcessTemperatureData,
            _ => throw new ArgumentException($"Invalid Job Type: {jobName}"),
        };
    }

    private async Task RunJobAsync(JobType jobType, ServiceProvider serviceProvider)
    {
        switch (jobType)
        {
            case JobType.ProcessTemperatureData:
                {
                    var processTemperatureDataJob = serviceProvider.GetService<ProcessTemperatureDataJob>();

                    await processTemperatureDataJob.RunAsync();
                    break;
                }
            default:
                {
                    throw new InvalidDataException($"Invalid JobType: {jobType}");
                }
        }
    }
}