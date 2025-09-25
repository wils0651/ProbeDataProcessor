using ProbeDataProcessor.Models;

namespace ProbeDataProcessor.Contracts
{
    public interface IGarageDistanceDataService
    {
        Task DeleteGarageDistances(List<GarageDistance> garageDistances);
        Task<List<GarageDistance>> GetGarageDistancesAsync();
    }
}
