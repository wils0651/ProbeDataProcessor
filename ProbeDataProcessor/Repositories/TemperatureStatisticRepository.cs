using ProbeDataProcessor.Contracts;
using ProbeDataProcessor.Models;

namespace ProbeDataProcessor.Repositories
{
    public class TemperatureStatisticRepository : ITemperatureStatisticRepository
    {
        private readonly DatabaseContext _context;

        public TemperatureStatisticRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task AddAndSave(TemperatureStatistic temperatureStatistic)
        {
            _context.TemperatureStatistic.Add(temperatureStatistic);
            await _context.SaveChangesAsync();
        }
    }
}
