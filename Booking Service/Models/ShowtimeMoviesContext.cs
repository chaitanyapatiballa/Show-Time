using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Booking_Service.Models;

public partial class ShowtimeMoviesContext : DbContext
{
    public ShowtimeMoviesContext()
    {
    }

    public ShowtimeMoviesContext(DbContextOptions<ShowtimeMoviesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Theater> Theaters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Showtime_movies;Username=postgres;Password=Admin");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasIndex(e => e.MovieId, "IX_Bookings_MovieId");

            entity.HasIndex(e => e.TheaterId, "IX_Bookings_TheaterId");

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'pending'::character varying");

            entity.HasOne(d => d.Movie).WithMany(p => p.Bookings).HasForeignKey(d => d.MovieId);

            entity.HasOne(d => d.Theater).WithMany(p => p.Bookings).HasForeignKey(d => d.TheaterId);
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasMany(d => d.Theaters).WithMany(p => p.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "MovieTheater",
                    r => r.HasOne<Theater>().WithMany().HasForeignKey("TheaterId"),
                    l => l.HasOne<Movie>().WithMany().HasForeignKey("MovieId"),
                    j =>
                    {
                        j.HasKey("MovieId", "TheaterId");
                        j.ToTable("MovieTheaters");
                        j.HasIndex(new[] { "TheaterId" }, "IX_MovieTheaters_TheaterId");
                    });
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
