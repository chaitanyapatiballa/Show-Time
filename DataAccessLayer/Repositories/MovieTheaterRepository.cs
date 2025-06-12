using DBModels.Db;


namespace TheaterService.Repositories
{
    public class MovieTheaterRepository
    {
        private readonly AppDbContext _context;

        public MovieTheaterRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MovieTheater> AssignMovieToTheater(MovieTheater assignment)   
        {
            _context.MovieTheaters.Add(assignment);
            await _context.SaveChangesAsync();
            return assignment;
        }

        public async Task<List<MovieTheater>> GetAllAssignmentsAsync()
        {
            return await Task.FromResult(_context.MovieTheaters.ToList());
        }
    }
}
