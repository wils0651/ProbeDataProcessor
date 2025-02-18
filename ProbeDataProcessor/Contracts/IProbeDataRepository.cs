using ProbeDataProcessor.Models;

namespace ProbeDataProcessor.Contracts
{
    public interface IProbeDataRepository
    {
        Task DeleteList(List<ProbeData> probeData);
        Task<List<ProbeData>> GetAllPastProbeData(int probeId);
    }
}
