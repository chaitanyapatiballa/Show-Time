﻿using Microsoft.EntityFrameworkCore;

namespace DBModels.Models 
{
    public partial class AppDbContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movietheater>(entity =>
            {
                entity.ToTable("movietheaters");

                entity.HasKey(mt => new { mt.Movieid, mt.Theaterid })       
                      .HasName("movietheater_pkey");

                entity.Property(mt => mt.Movieid).HasColumnName("movieid");
                entity.Property(mt => mt.Theaterid).HasColumnName("theaterid");

                entity.HasOne(mt => mt.Movie)
                      .WithMany(m => m.Movietheater)
                      .HasForeignKey(mt => mt.Movieid)
                      .HasConstraintName("movietheaters_movieid_fkey");

                entity.HasOne(mt => mt.Theater)
                      .WithMany(t => t.Movietheater)
                      .HasForeignKey(mt => mt.Theaterid)
                      .HasConstraintName("movietheaters_theaterid_fkey");
            });
        }
    }
}
