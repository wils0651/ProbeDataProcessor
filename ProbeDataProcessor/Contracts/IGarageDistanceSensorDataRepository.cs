using ProbeDataProcessor.Models;

namespace ProbeDataProcessor.Contracts
{
    public interface IGarageDistanceSensorDataRepository
    {
        Task DeleteListAsync(List<GarageDistance> garageDistances);
        Task<List<GarageDistance>> GetPastGarageDataAsync();
    }
}
