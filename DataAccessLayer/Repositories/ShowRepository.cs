using DBModels.Db;
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
        public async Task<ShowTemplate> AddTemplateAsync(ShowTemplate template)
        {
                _context.ShowTemplates.Add(template);
                await _context.SaveChangesAsync();
                return template;
        }

        public async Task<ShowInstance> AddInstanceAsync(ShowInstance instance)
        { 
                _context.ShowInstances.Add(instance);
                await _context.SaveChangesAsync();
                return instance;
        }

        public async Task<ShowInstance?> GetShowInstance(int movieId, int theaterId, DateTime showTime)
        {
                return await _context.ShowInstances
                    .Include(si => si.ShowTemplate)
                    .FirstOrDefaultAsync(si =>
                        si.ShowTime == showTime &&
                        si.ShowTemplate.MovieId == movieId &&
                        si.ShowTemplate.TheaterId == theaterId);
        }
        }
    }


