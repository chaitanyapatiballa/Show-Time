using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DBModels.Db
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; }

        public virtual DbSet<Movie> Movies { get; set; }

        public virtual DbSet<Payment> Payments { get; set; }

        public virtual DbSet<Theater> Theaters { get; set; }

        public virtual DbSet<MovieTheater> MovieTheaters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

            => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ShowtimeMovies;Username=postgres;Password=Admin");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasIndex(e => e.MovieId, "IX_Bookings_MovieId");
                entity.HasIndex(e => e.TheaterId, "IX_Bookings_TheaterId");

                entity.HasOne(d => d.Movie).WithMany(p => p.Bookings).HasForeignKey(d => d.MovieId);
                entity.HasOne(d => d.Theater).WithMany(p => p.Bookings).HasForeignKey(d => d.TheaterId);
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.Property(e => e.Duration).HasMaxLength(50);
            });

            modelBuilder.Entity<MovieTheater>(entity =>
            {
                entity.HasKey(e => new { e.MovieId, e.TheaterId });

                entity.HasIndex(e => e.TheaterId, "IX_MovieTheaters_TheaterId");

                entity.HasOne(mt => mt.Movie)
                      .WithMany(m => m.MovieTheaters)
                      .HasForeignKey(mt => mt.MovieId);

                entity.HasOne(mt => mt.Theater)
                      .WithMany(t => t.MovieTheaters)
                      .HasForeignKey(mt => mt.TheaterId);

                entity.ToTable("MovieTheaters");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasIndex(e => e.BookingId, "IX_Payments_BookingId").IsUnique();

                entity.HasOne(d => d.Booking).WithOne(p => p.Payment).HasForeignKey<Payment>(d => d.BookingId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
