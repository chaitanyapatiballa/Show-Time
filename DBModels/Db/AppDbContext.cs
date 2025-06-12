using System;
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
            // ✅ Booking Entity
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.BookingId);

                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.SeatNumber).HasMaxLength(10);
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.ShowTime).IsRequired();
                entity.Property(e => e.BookingTime).IsRequired();

                entity.HasIndex(e => e.MovieId).HasDatabaseName("IX_Bookings_MovieId");
                entity.HasIndex(e => e.TheaterId).HasDatabaseName("IX_Bookings_TheaterId");
                entity.HasIndex(e => e.PaymentId).HasDatabaseName("IX_Bookings_PaymentId");

                entity.HasOne(d => d.Movie)
                      .WithMany(p => p.Bookings)
                      .HasForeignKey(d => d.MovieId);

                entity.HasOne(d => d.Theater)
                      .WithMany(p => p.Bookings)
                      .HasForeignKey(d => d.TheaterId);

                entity.HasOne(d => d.Payment)
                      .WithMany()
                      .HasForeignKey(d => d.PaymentId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // ✅ Movie Entity
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(e => e.MovieId);
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Duration).HasMaxLength(50);
            });

            // ✅ Theater Entity
            modelBuilder.Entity<Theater>(entity =>
            {
                entity.HasKey(e => e.TheaterId);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Location).IsRequired();
            });

            // ✅ MovieTheater (Join Table)
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

            // ✅ Payment Entity
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.PaymentId);
                entity.Property(e => e.PaymentId).ValueGeneratedOnAdd();

                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Amount).HasColumnType("decimal(10,2)");
                entity.Property(e => e.PaymentTime).IsRequired();
                entity.Property(e => e.IsSuccessful).IsRequired();

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

