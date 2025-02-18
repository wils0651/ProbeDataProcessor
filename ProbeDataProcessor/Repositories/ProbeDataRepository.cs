using Microsoft.EntityFrameworkCore;
using ProbeDataProcessor.Contracts;
using ProbeDataProcessor.Models;

namespace ProbeDataProcessor.Repositories
{
    public class ProbeDataRepository(DatabaseContext context) : IProbeDataRepository
    {
        private readonly DatabaseContext _context = context;

        public async Task<List<ProbeData>> GetAllPastProbeData(int probeId)
        {
            var endDate = DateTime.Now.AddDays(-2).Date;

            return await _context.ProbeData
                .Include(pd => pd.Probe)
                .Where(pd => pd.ProbeId == probeId)
                .Where(pd => pd.CreatedDate < endDate)
                .OrderByDescending(pd => pd.CreatedDate)
                .ToListAsync();
        }

        public async Task DeleteList(List<ProbeData> probeData)
        {
            _context.ProbeData.RemoveRange(probeData);
            await _context.SaveChangesAsync();
        }
    }
}
