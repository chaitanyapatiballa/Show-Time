using DBModels.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class ShowRepository
    {
        private readonly AppDbContext _context;

        public ShowRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Showtemplate> AddTemplateAsync(Showtemplate template)
        {
            _context.ShowTemplates.Add(template);
            await _context.SaveChangesAsync();
            return template;
        }

        public async Task<Showinstance> AddInstanceAsync(Showinstance instance)
        {
            _context.ShowInstances.Add(instance);
            await _context.SaveChangesAsync();
            return instance;
        }

        public async Task<Showinstance?> GetShowInstance(int movieId, int theaterId, DateTime showTime)
        {
            return await _context.ShowInstances
                .Include(si => si.Showtemplate)
                .FirstOrDefaultAsync(si =>
                    si.ShowTime == showTime && 
                    si.Showtemplate.Movieid == movieId &&
                    si.Showtemplate.Theaterid == theaterId);
        }
    }
}


