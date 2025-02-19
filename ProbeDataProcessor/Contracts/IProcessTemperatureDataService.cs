
using ProbeDataProcessor.Models;

namespace ProbeDataProcessor.Contracts
{
    public interface IProcessTemperatureDataService
    {
        Task DeleteProbeDataAsync(List<ProbeData> probeData);
        Task<Dictionary<DateTime, List<ProbeData>>> GetProbeDataByDateAsync(int probeId);
        Task<List<int>> GetProbeIdsAsync();
        Task ProcessAndSaveStatisticsAsync(DateTime measurementDate, List<ProbeData> probeData);
    }
}
