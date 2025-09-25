using Microsoft.EntityFrameworkCore;
using ProbeDataProcessor.Contracts;
using ProbeDataProcessor.Models;

namespace ProbeDataProcessor.Repositories
{
    public class GarageDistanceSensorDataRepository(DatabaseContext context) : IGarageDistanceSensorDataRepository
    {
        private readonly DatabaseContext _context = context;

        public async Task<List<GarageDistance>> GetPastGarageDataAsync()
        {
            var twoDaysAgo = DateTime.Now.AddDays(-2).Date;

            var endDate = DateTime.SpecifyKind(twoDaysAgo, DateTimeKind.Local).ToUniversalTime();

            return await _context.GarageDistance
                .Where(gd => gd.CreatedDate < endDate)
                .ToListAsync();
        }

        public async Task DeleteListAsync(List<GarageDistance> garageDistances)
        {
            _context.GarageDistance.RemoveRange(garageDistances);
            await _context.SaveChangesAsync();
        }
    }
}
