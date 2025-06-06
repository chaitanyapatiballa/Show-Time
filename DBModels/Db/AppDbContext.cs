using Microsoft.EntityFrameworkCore;
namespace DBModels.Db
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<Theater> Theaters => Set<Theater>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<MovieTheater> MovieTheaters => Set<MovieTheater>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieTheater>().HasKey(mt => new { mt.MovieId, mt.TheaterId });

            modelBuilder.Entity<MovieTheater>()
                .HasOne(mt => mt.Movie)
                .WithMany(m => m.MovieTheaters)
                .HasForeignKey(mt => mt.MovieId);

            modelBuilder.Entity<MovieTheater>()
                .HasOne(mt => mt.Theater)
                .WithMany(t => t.MovieTheaters)
                .HasForeignKey(mt => mt.TheaterId);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.BookingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}