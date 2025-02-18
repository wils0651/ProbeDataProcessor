using ProbeDataProcessor.Models;

namespace ProbeDataProcessor.Contracts
{
    public interface IProbeDataRepository
    {
        Task<List<ProbeData>> GetAllProbeData(int probeId);
    }
}
