using System;
using Microsoft.EntityFrameworkCore;

namespace DBModels.Db
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<Movie> Movies { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Theater> Theaters { get; set; } = null!;
        public virtual DbSet<MovieTheater> MovieTheaters { get; set; } = null!;
        public DbSet<BillingSummary> BillingSummaries { get; set; }
        public DbSet<ShowTemplate> ShowTemplates { get; set; }
        public DbSet<ShowInstance> ShowInstances { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ShowtimeMovies;Username=postgres;Password=Admin");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //  Booking
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.BookingId);

                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.SeatNumber).HasMaxLength(10);
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.ShowTime).IsRequired();
                entity.Property(e => e.BookingTime).IsRequired();

                entity.HasIndex(e => e.MovieId);
                entity.HasIndex(e => e.TheaterId);
                entity.HasIndex(e => e.PaymentId);

                entity.HasOne(d => d.Movie)
                      .WithMany(p => p.Bookings)
                      .HasForeignKey(d => d.MovieId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Theater)
                      .WithMany(p => p.Bookings)
                      .HasForeignKey(d => d.TheaterId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Payment)
                      .WithMany()
                      .HasForeignKey(d => d.PaymentId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            //  Movie
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(e => e.MovieId);

                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Duration).HasMaxLength(50);
            });

            //  Theater
            modelBuilder.Entity<Theater>(entity =>
            {
                entity.HasKey(e => e.TheaterId);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Location).IsRequired();
            });


            //  MovieTheater (many-to-many)
            modelBuilder.Entity<MovieTheater>(entity =>
            {
                entity.HasKey(e => new { e.MovieId, e.TheaterId });

                entity.HasIndex(e => e.TheaterId);

                entity.HasOne(mt => mt.Movie)
                      .WithMany(m => m.MovieTheaters)
                      .HasForeignKey(mt => mt.MovieId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(mt => mt.Theater)
                      .WithMany(t => t.MovieTheaters)
                      .HasForeignKey(mt => mt.TheaterId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.ToTable("MovieTheaters");
            });

            // ✅ Payment
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.PaymentId);

                entity.Property(e => e.PaymentId).ValueGeneratedOnAdd();

                entity.Property(e => e.UserId)
                      .IsRequired();

                entity.Property(e => e.AmountPaid)
                      .HasColumnType("decimal(10,2)")
                      .IsRequired();

                entity.Property(e => e.PaymentDate) 
                      .IsRequired();

                entity.Property(e => e.PaymentMethod)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.HasIndex(e => e.BookingId)
                      .IsUnique();

                entity.HasOne(p => p.Booking)   
                      .WithOne(b => b.Payment)
                      .HasForeignKey<Payment>(p => p.BookingId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ShowTemplate>()
            .HasMany(t => t.ShowInstances)
            .WithOne(i => i.ShowTemplate)
            .HasForeignKey(i => i.ShowTemplateId);

        }
    }
}
