using Microsoft.EntityFrameworkCore;
using TheaterService.Models;

namespace TheaterService.Data
{
    public class TheaterDbContext : DbContext
    {
        public TheaterDbContext(DbContextOptions<TheaterDbContext> options) : base(options) { }

        public DbSet<Theater> Theaters { get; set; }
    }
       
}
