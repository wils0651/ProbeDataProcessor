using Microsoft.EntityFrameworkCore;
using ProbeDataProcessor.Contracts;
using ProbeDataProcessor.Models;

namespace ProbeDataProcessor.Repositories
{
    public class ProbeRepository : IProbeRepository
    {
        private readonly DatabaseContext _context;

        public ProbeRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Probe>> ListProbes()
        {
            return await _context.Probe
                .ToListAsync();
        }
    }
}
