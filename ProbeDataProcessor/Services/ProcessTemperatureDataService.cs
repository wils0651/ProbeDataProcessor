using ProbeDataProcessor.Contracts;
using ProbeDataProcessor.Models;

namespace ProbeDataProcessor.Services
{
    public class ProcessTemperatureDataService : IProcessTemperatureDataService
    {
        private readonly IProbeRepository _probeRepository;
        private readonly IProbeDataRepository _probeDataRepository;
        private readonly ITemperatureStatisticRepository _temperatureStatisticRepository;

        public ProcessTemperatureDataService(
            IProbeRepository probeRepository,
            IProbeDataRepository probeDataRepository,
            ITemperatureStatisticRepository temperatureStatisticRepository)
        {
            _probeRepository = probeRepository;
            _probeDataRepository = probeDataRepository;
            _temperatureStatisticRepository = temperatureStatisticRepository;
        }

        public async Task<List<int>> GetProbeIds()
        {
            var probes = await _probeRepository.ListProbes();

            return probes
                .Select(p => p.ProbeId)
                .ToList();
        }

        public async Task<Dictionary<DateTime, List<ProbeData>>> GetProbeDataByDate(int probeId)
        {
            var probeDataByDate = new Dictionary<DateTime, List<ProbeData>>();

            var probeData = await _probeDataRepository.GetAllPastProbeData(probeId);

            while (probeData.Count > 0)
            {
                var date = probeData.First().CreatedDate.Date;

                probeDataByDate[date] = probeData
                    .Where(pd => pd.CreatedDate.Date == date)
                    .ToList();

                probeData = probeData
                    .Where(pd => pd.CreatedDate.Date != date)
                    .ToList();
            }

            return probeDataByDate;
        }

        public async Task ProcessAndSaveStatistics(DateTime measurementDate, List<ProbeData> probeData)
        {
            var temperatureStatistic = CreateTemperatureStatistic(measurementDate, probeData);

            await _temperatureStatisticRepository.AddAndSave(temperatureStatistic);
        }

        public async Task DeleteProbeData(List<ProbeData> probeData)
        {
            await _probeDataRepository.DeleteList(probeData);
        }

        private static TemperatureStatistic CreateTemperatureStatistic(DateTime measurementDate, List<ProbeData> probeData)
        {
            var count = probeData.Count;

            var meanTemperature = probeData
                .Select(pd => pd.Temperature)
                .Average();

            var standardDeviation = CalculateStandardDeviation(probeData, meanTemperature);

            var maxTemperature = probeData
                .Select(pd => pd.Temperature)
                .Max();

            var minTemperature = probeData
                .Select(pd => pd.Temperature)
                .Min();

            return new TemperatureStatistic
            {
                MeasurementDate = measurementDate,
                DataCount = count,
                Mean = meanTemperature,
                StandardDeviation = standardDeviation,
                Maximum = maxTemperature,
                Minimum = minTemperature,
                ProbeId = probeData.First().ProbeId
            };
        }

        private static decimal CalculateStandardDeviation(List<ProbeData> probeData, decimal meanTemperature)
        {
            var sumOfSquares = probeData
                .Select(pd => Math.Pow((double)pd.Temperature - (double)meanTemperature, 2))
                .Sum();

            return (decimal)Math.Sqrt(sumOfSquares / probeData.Count);
        }
    }
}
