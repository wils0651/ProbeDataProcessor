using ProbeDataProcessor.Models;

namespace ProbeDataProcessor.Contracts
{
    public interface ITemperatureStatisticRepository
    {
        Task AddAndSave(TemperatureStatistic temperatureStatistic);
    }
}
