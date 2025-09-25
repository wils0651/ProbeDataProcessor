using ProbeDataProcessor.Contracts;
using ProbeDataProcessor.Models;

namespace ProbeDataProcessor.Services
{
    public class GarageDistanceDataService : IGarageDistanceDataService
    {
        private readonly IGarageDistanceSensorDataRepository _garageDistanceSensorDataRepository;

        public GarageDistanceDataService(IGarageDistanceSensorDataRepository garageDistanceSensorDataRepository)
        {
            _garageDistanceSensorDataRepository = garageDistanceSensorDataRepository;
        }

        public async Task<List<GarageDistance>> GetGarageDistancesAsync()
        {
            return await _garageDistanceSensorDataRepository.GetPastGarageDataAsync();
        }

        public async Task DeleteGarageDistances(List<GarageDistance> garageDistances)
        {
            await _garageDistanceSensorDataRepository.DeleteListAsync(garageDistances);
        }
    }
}
