using ProbeDataProcessor.Models;

namespace ProbeDataProcessor.Contracts
{
    public interface IProbeRepository
    {
        Task<List<Probe>> ListProbes();
    }
}
