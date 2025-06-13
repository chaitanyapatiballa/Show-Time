using DBModels.Db;
using Microsoft.EntityFrameworkCore;


namespace TheaterService.Repositories
{
    public class MovieTheaterRepository
    {
        private readonly AppDbContext _context;

        public MovieTheaterRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MovieTheater>> GetAllAssignmentsAsync()
        {
            return await _context.MovieTheaters
                .Include(mt => mt.Movie)
                .Include(mt => mt.Theater)
                .ToListAsync();
        }

        public async Task AddAssignmentAsync(MovieTheater assignment)
        {
            _context.MovieTheaters.Add(assignment);
            await _context.SaveChangesAsync();
        }
    }
}
