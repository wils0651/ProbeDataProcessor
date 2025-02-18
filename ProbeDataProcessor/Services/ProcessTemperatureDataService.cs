using ProbeDataProcessor.Contracts;

namespace ProbeDataProcessor.Services
{
    public class ProcessTemperatureDataService : IProcessTemperatureDataService
    {
        private readonly IProbeRepository _probeRepository;
        private readonly IProbeDataRepository _probeDataRepository;

        public ProcessTemperatureDataService(IProbeRepository probeRepository, IProbeDataRepository probeDataRepository)
        {
            this._probeRepository = probeRepository;
            this._probeDataRepository = probeDataRepository;
        }

        public async Task<List<int>> GetProbeIds()
        {
            var probes = await _probeRepository.ListProbes();

            return probes
                .Select(p => p.ProbeId)
                .ToList();
        }


    }
}
