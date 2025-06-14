﻿using System;
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

    public virtual DbSet<Billingsummary> Billingsummaries { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Showinstance> Showinstances { get; set; }

    public virtual DbSet<Showtemplate> Showtemplates { get; set; }

    public virtual DbSet<Theater> Theaters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=showtime_db;Username=postgres;Password=Admin");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Billingsummary>(entity =>
        {
            entity.HasKey(e => e.Billingsummaryid).HasName("billingsummaries_pkey");

            entity.ToTable("billingsummaries");

            entity.Property(e => e.Billingsummaryid).HasColumnName("billingsummaryid");
            entity.Property(e => e.Baseamount)
                .HasPrecision(10, 2)
                .HasColumnName("baseamount");
            entity.Property(e => e.Bookingid).HasColumnName("bookingid");
            entity.Property(e => e.Discount)
                .HasPrecision(10, 2)
                .HasColumnName("discount");
            entity.Property(e => e.Gst)
                .HasPrecision(10, 2)
                .HasColumnName("gst");
            entity.Property(e => e.Servicefee)
                .HasPrecision(10, 2)
                .HasColumnName("servicefee");
            entity.Property(e => e.Totalamount)
                .HasPrecision(10, 2)
                .HasColumnName("totalamount");

            entity.HasOne(d => d.Booking).WithMany(p => p.Billingsummaries)
                .HasForeignKey(d => d.Bookingid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("billingsummaries_bookingid_fkey");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Bookingid).HasName("bookings_pkey");

            entity.ToTable("bookings");

            entity.Property(e => e.Bookingid).HasColumnName("bookingid");
            entity.Property(e => e.Bookingtime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("bookingtime");
            entity.Property(e => e.Movieid).HasColumnName("movieid");
            entity.Property(e => e.Seatnumber)
                .HasMaxLength(10)
                .HasColumnName("seatnumber");
            entity.Property(e => e.Showtime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("showtime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.Theaterid).HasColumnName("theaterid");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Movie).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.Movieid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("bookings_movieid_fkey");

            entity.HasOne(d => d.Theater).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.Theaterid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("bookings_theaterid_fkey");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.Movieid).HasName("movies_pkey");

            entity.ToTable("movies");

            entity.Property(e => e.Movieid).HasColumnName("movieid");
            entity.Property(e => e.Duration)
                .HasMaxLength(50)
                .HasColumnName("duration");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");

            entity.HasMany(d => d.Theaters).WithMany(p => p.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "Movietheater",
                    r => r.HasOne<Theater>().WithMany()
                        .HasForeignKey("Theaterid")
                        .HasConstraintName("movietheaters_theaterid_fkey"),
                    l => l.HasOne<Movie>().WithMany()
                        .HasForeignKey("Movieid")
                        .HasConstraintName("movietheaters_movieid_fkey"),
                    j =>
                    {
                        j.HasKey("Movieid", "Theaterid").HasName("movietheaters_pkey");
                        j.ToTable("movietheaters");
                        j.IndexerProperty<int>("Movieid").HasColumnName("movieid");
                        j.IndexerProperty<int>("Theaterid").HasColumnName("theaterid");
                    });
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Paymentid).HasName("payments_pkey");

            entity.ToTable("payments");

            entity.HasIndex(e => e.Bookingid, "payments_bookingid_key").IsUnique();

            entity.Property(e => e.Paymentid).HasColumnName("paymentid");
            entity.Property(e => e.Amountpaid)
                .HasPrecision(10, 2)
                .HasColumnName("amountpaid");
            entity.Property(e => e.Bookingid).HasColumnName("bookingid");
            entity.Property(e => e.Paymentdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("paymentdate");
            entity.Property(e => e.Paymentmethod)
                .HasMaxLength(50)
                .HasColumnName("paymentmethod");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Booking).WithOne(p => p.Payment)
                .HasForeignKey<Payment>(d => d.Bookingid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("payments_bookingid_fkey");
        });

        modelBuilder.Entity<Showinstance>(entity =>
        {
            entity.HasKey(e => e.Showinstanceid).HasName("showinstances_pkey");

            entity.ToTable("showinstances");

            entity.Property(e => e.Showinstanceid).HasColumnName("showinstanceid");
            entity.Property(e => e.Availableseats).HasColumnName("availableseats");
            entity.Property(e => e.Showdate).HasColumnName("showdate");
            entity.Property(e => e.Showtemplateid).HasColumnName("showtemplateid");
            entity.Property(e => e.Showtime).HasColumnName("showtime");

            entity.HasOne(d => d.Showtemplate).WithMany(p => p.Showinstances)
                .HasForeignKey(d => d.Showtemplateid)
                .HasConstraintName("showinstances_showtemplateid_fkey");
        });

        modelBuilder.Entity<Showtemplate>(entity =>
        {
            entity.HasKey(e => e.Showtemplateid).HasName("showtemplates_pkey");

            entity.ToTable("showtemplates");

            entity.Property(e => e.Showtemplateid).HasColumnName("showtemplateid");
            entity.Property(e => e.Baseprice)
                .HasPrecision(10, 2)
                .HasColumnName("baseprice");
            entity.Property(e => e.Format)
                .HasMaxLength(50)
                .HasColumnName("format");
            entity.Property(e => e.Language)
                .HasMaxLength(50)
                .HasColumnName("language");
            entity.Property(e => e.Movieid).HasColumnName("movieid");
            entity.Property(e => e.Theaterid).HasColumnName("theaterid");

            entity.HasOne(d => d.Movie).WithMany(p => p.Showtemplates)
                .HasForeignKey(d => d.Movieid)
                .HasConstraintName("showtemplates_movieid_fkey");

            entity.HasOne(d => d.Theater).WithMany(p => p.Showtemplates)
                .HasForeignKey(d => d.Theaterid)
                .HasConstraintName("showtemplates_theaterid_fkey");
        });

        modelBuilder.Entity<Theater>(entity =>
        {
            entity.HasKey(e => e.Theaterid).HasName("theaters_pkey");

            entity.ToTable("theaters");

            entity.Property(e => e.Theaterid).HasColumnName("theaterid");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
