
using ProbeDataProcessor.Models;

namespace ProbeDataProcessor.Contracts
{
    public interface IProcessTemperatureDataService
    {
        Task DeleteProbeData(List<ProbeData> probeData);
        Task<Dictionary<DateTime, List<ProbeData>>> GetProbeDataByDate(int probeId);
        Task<List<int>> GetProbeIds();
        Task ProcessAndSaveStatistics(DateTime measurementDate, List<ProbeData> probeData);
    }
}
