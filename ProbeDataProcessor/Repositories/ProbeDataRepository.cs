using Microsoft.EntityFrameworkCore;
using ProbeDataProcessor.Contracts;
using ProbeDataProcessor.Models;

namespace ProbeDataProcessor.Repositories
{
    public class ProbeDataRepository(DatabaseContext context) : IProbeDataRepository
    {
        private readonly DatabaseContext _context = context;

        public async Task<List<ProbeData>> GetAllProbeData(int probeId)
        {
            var endDate = DateTime.Now.AddDays(-2).Date;

            return await _context.ProbeData
                .Include(pd => pd.Probe)
                .Where(pd => pd.ProbeId == probeId)
                .Where(pd => pd.CreatedDate < endDate)
                .ToListAsync();
        }
    }
}
