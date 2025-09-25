using ProbeDataProcessor.Contracts;

namespace ProbeDataProcessor.Jobs
{
    public class DeleteGarageDistanceSensorDataJob : IJob
    {
        private readonly IGarageDistanceDataService _garageDistanceDataService;

        public DeleteGarageDistanceSensorDataJob(IGarageDistanceDataService garageDistanceDataService)
        {
            _garageDistanceDataService = garageDistanceDataService;
        }

        public async Task RunAsync()
        {
            // Get Data
            var oldGarageDistances = await _garageDistanceDataService.GetGarageDistancesAsync();

            // Delete Data
            await _garageDistanceDataService.DeleteGarageDistances(oldGarageDistances);
        }
    }
}
