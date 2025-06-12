using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DBModels.Db
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

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
                entity.HasKey(e => e.TheaterId);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.SeatNumber).HasMaxLength(10);
                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasIndex(e => e.MovieId).HasDatabaseName("IX_Bookings_MovieId");
                entity.HasIndex(e => e.TheaterId).HasDatabaseName("IX_Bookings_TheaterId");
                entity.HasIndex(e => e.PaymentId).HasDatabaseName("IX_Bookings_PaymentId");

                // Foreign keys
                entity.HasOne(d => d.Movie)
                      .WithMany(p => p.Bookings)
                      .HasForeignKey(d => d.MovieId);

                entity.HasOne(d => d.Theater)
                      .WithMany(p => p.Bookings)
                      .HasForeignKey(d => d.TheaterId);

                entity.HasOne(d => d.Payment)
                      .WithMany()
                      .HasForeignKey(d => d.PaymentId);
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(e => e.TheaterId);
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Duration).HasMaxLength(50);
            });

            modelBuilder.Entity<Theater>(entity =>
            {
                entity.HasKey(e => e.TheaterId);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Location).IsRequired();
            });

            modelBuilder.Entity<MovieTheater>(entity =>
            {
                entity.HasKey(e => new { e.MovieId, e.TheaterId });

                entity.HasIndex(e => e.TheaterId).HasDatabaseName("IX_MovieTheaters_TheaterId");

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
                entity.HasKey(e => e.PaymentId);

                entity.HasIndex(e => e.BookingId)
                      .IsUnique()
                      .HasDatabaseName("IX_Payments_BookingId");

                entity.HasOne(p => p.Booking)
                      .WithOne(b => b.Payment)
                      .HasForeignKey<Payment>(p => p.BookingId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

