using ProbeDataProcessor.Contracts;

namespace ProbeDataProcessor.Jobs
{
    public class ProcessTemperatureDataJob : IJob
    {
        private readonly IProcessTemperatureDataService _processTemperatureDataService;

        public ProcessTemperatureDataJob(IProcessTemperatureDataService processTemperatureDataService)
        {
            _processTemperatureDataService = processTemperatureDataService;
        }

        public async Task RunAsync()
        {
            var probeIds = await _processTemperatureDataService.GetProbeIdsAsync();

            foreach (var probeId in probeIds)
            {
                // Get the probe data and group by date
                var probeDataByDate = await _processTemperatureDataService.GetProbeDataByDateAsync(probeId);

                // Process and save statistics
                foreach (var probeData in probeDataByDate)
                {
                    var date = probeData.Key;
                    var temperatureData = probeData.Value;

                    await _processTemperatureDataService.ProcessAndSaveStatisticsAsync(date, temperatureData);

                    await _processTemperatureDataService.DeleteProbeDataAsync(probeData.Value);
                }
            }
        }
    }
}
