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

        // Get Job
        var jobName = args[0];
        var jobType = GetJobType(jobName);
        var job = GetJob(jobType, serviceProvider);

        // Run the job
        await job.RunAsync();
    }

    /// <summary>
    /// Get the JobType from the jobName.
    /// </summary>
    /// <param name="jobName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private static JobType GetJobType(string jobName)
    {
        return jobName.ToLower() switch
        {
            "processtemperaturedata" => JobType.ProcessTemperatureData,
            _ => throw new ArgumentException($"Invalid Job Type: {jobName}"),
        };
    }

    /// <summary>
    /// Get the job to run.
    /// </summary>
    /// <param name="jobType"></param>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    /// <exception cref="InvalidDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private static IJob GetJob(JobType jobType, IServiceProvider serviceProvider)
    {
        IJob job;
        switch (jobType)
        {
            case JobType.ProcessTemperatureData:
                {
                    job = serviceProvider.GetService<ProcessTemperatureDataJob>();
                    break;
                }
            default:
                {
                    throw new InvalidDataException($"Invalid JobType: {jobType}");
                }
        }

        if (job == null)
        {
            throw new InvalidOperationException("Job is null");
        }

        return job;
    }
}
