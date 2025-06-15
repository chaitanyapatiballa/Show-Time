using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DBModels.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; } = null!;
    public virtual DbSet<Movie> Movies { get; set; } = null!;
    public virtual DbSet<Payment> Payments { get; set; } = null!;
    public virtual DbSet<Theater> Theaters { get; set; } = null!;
    public virtual DbSet<MovieTheater> MovieTheaters { get; set; } = null!;
    public DbSet<Billingsummary> BillingSummaries { get; set; }
    public DbSet<Showtemplate> ShowTemplates { get; set; }
    public DbSet<Showinstance> ShowInstances { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=showtime_db;Username=postgres;Password=Admin");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("public");

        // Booking
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("bookings"); 
            entity.HasKey(e => e.Bookingid);
            entity.Property(e => e.Userid).IsRequired();
            entity.Property(e => e.Seatnumber).HasMaxLength(10);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Showtime).IsRequired();
            entity.Property(e => e.Bookingtime).IsRequired();

            entity.HasIndex(e => e.Movieid);
            entity.HasIndex(e => e.Theaterid);
            entity.HasIndex(e => e.Payment);

            entity.HasOne(d => d.Movie)
                  .WithMany(p => p.Bookings)
                  .HasForeignKey(d => d.Movieid)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Theater)
                  .WithMany(p => p.Bookings)
                  .HasForeignKey(d => d.Theaterid)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Payment)
                  .WithMany()
                  .HasForeignKey(d => d.Payment)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Movie
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("movies"); 
            entity.HasKey(e => e.Movieid);
            entity.Property(e => e.Title).IsRequired();
            entity.Property(e => e.Duration).HasMaxLength(50);
        });

        // Theater
        modelBuilder.Entity<Theater>(entity =>
        {
            entity.ToTable("theaters"); 
            entity.HasKey(e => e.Theaterid);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Location).IsRequired();
        });

        // MovieTheater
        modelBuilder.Entity<MovieTheater>(entity =>
        {
            entity.ToTable("movietheaters"); 
            entity.HasKey(e => new { e.Movieid, e.Theaterid });

            entity.HasIndex(e => e.Theaterid);

            entity.HasOne(mt => mt.Movie)
                  .WithMany(m => m.movietheaters)
                  .HasForeignKey(mt => mt.Movieid)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(mt => mt.Theater)
                  .WithMany(t => t.movietheaters)
                  .HasForeignKey(mt => mt.Theaterid)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Payment
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("payments"); 
            entity.HasKey(e => e.Paymentid);
            entity.Property(e => e.Paymentid).ValueGeneratedOnAdd();
            entity.Property(e => e.Userid).IsRequired();
            entity.Property(e => e.AmountPaid).HasColumnType("decimal(10,2)").IsRequired();
            entity.Property(e => e.Paymentdate).IsRequired();
            entity.Property(e => e.Paymentmethod).HasMaxLength(50).IsRequired();

            entity.HasIndex(e => e.Bookingid).IsUnique();

            entity.HasOne(p => p.Booking)
                  .WithOne(b => b.Payment)
                  .HasForeignKey<Payment>(p => p.Bookingid)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // BillingSummary
        modelBuilder.Entity<Billingsummary>(entity =>
        {
            entity.ToTable("billingsummaries"); 
            entity.HasKey(e => e.Billingsummaryid);
            // Add properties if needed
        });

        // ShowTemplate
        modelBuilder.Entity<Showtemplate>(entity =>
        {
            entity.ToTable("showtemplates"); 
            entity.HasKey(e => e.Showtemplateid);

            entity.HasMany(t => t.Showinstances)
                  .WithOne(i => i.Showtemplate)
                  .HasForeignKey(i => i.Showtemplateid);
        });

        // ShowInstance
        modelBuilder.Entity<Showinstance>(entity =>
        {
            entity.ToTable("showinstances"); 
            entity.HasKey(e => e.Showinstanceid);
            // Add properties if needed
        });
    }
}
