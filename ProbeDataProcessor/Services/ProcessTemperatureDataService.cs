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

        /// <summary>
        /// Gets all the probe IDs.
        /// </summary>
        /// <returns></returns>
        public async Task<List<int>> GetProbeIdsAsync()
        {
            var probes = await _probeRepository.ListProbes();

            return probes
                .Select(p => p.ProbeId)
                .ToList();
        }

        /// <summary>
        /// Groups probe data by date.
        /// </summary>
        /// <param name="probeId"></param>
        /// <returns></returns>
        public async Task<Dictionary<DateTime, List<ProbeData>>> GetProbeDataByDateAsync(int probeId)
        {
            var probeDataByDate = new Dictionary<DateTime, List<ProbeData>>();

            var probeData = await _probeDataRepository.GetAllPastProbeData(probeId);

            while (probeData.Count > 0)
            {
                var date = probeData
                    .OrderBy(pd => pd.CreatedDate)
                    .First().CreatedDate.Date;

                probeDataByDate[date] = probeData
                    .Where(pd => pd.CreatedDate.Date == date)
                    .ToList();

                probeData = probeData
                    .Where(pd => pd.CreatedDate.Date != date)
                    .ToList();
            }

            return probeDataByDate;
        }

        /// <summary>
        /// Process and saves temperature statistics.
        /// </summary>
        /// <param name="measurementDate"></param>
        /// <param name="probeData"></param>
        /// <returns></returns>
        public async Task ProcessAndSaveStatisticsAsync(DateTime measurementDate, List<ProbeData> probeData)
        {
            var temperatureStatistic = CreateTemperatureStatistic(measurementDate, probeData);

            await _temperatureStatisticRepository.AddAndSave(temperatureStatistic);
        }

        /// <summary>
        /// Deletes a list of ProbeData.
        /// </summary>
        /// <param name="probeData"></param>
        /// <returns></returns>
        public async Task DeleteProbeDataAsync(List<ProbeData> probeData)
        {
            await _probeDataRepository.DeleteList(probeData);
        }

        /// <summary>
        /// Creates a TemperatureStatistic containing mean, standard deviation, max, min, and count for a given
        /// probe on a given day.
        /// </summary>
        /// <param name="measurementDate"></param>
        /// <param name="probeData"></param>
        /// <returns></returns>
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
                MeasurementDate = measurementDate.ToUniversalTime(),
                DataCount = count,
                Mean = meanTemperature,
                StandardDeviation = standardDeviation,
                Maximum = maxTemperature,
                Minimum = minTemperature,
                ProbeId = probeData.First().ProbeId
            };
        }

        /// <summary>
        /// Calculates the standard deviation for a list of probeDatas and a given mean temperature.
        /// </summary>
        /// <param name="probeDatas"></param>
        /// <param name="meanTemperature"></param>
        /// <returns></returns>
        private static decimal CalculateStandardDeviation(List<ProbeData> probeDatas, decimal meanTemperature)
        {
            var sumOfSquares = probeDatas
                .Select(pd => Math.Pow((double)pd.Temperature - (double)meanTemperature, 2))
                .Sum();

            return (decimal)Math.Sqrt(sumOfSquares / probeDatas.Count);
        }
    }
}
