using Microsoft.EntityFrameworkCore;
using DBModels.Models;

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
                      .HasName("movietheaters_pkey");

                entity.Property(mt => mt.Movieid).HasColumnName("movieid");
                entity.Property(mt => mt.Theaterid).HasColumnName("theaterid");

                entity.HasOne(mt => mt.Movie)
                      .WithMany(m => m.MovieTheaters)
                      .HasForeignKey(mt => mt.Movieid)
                      .HasConstraintName("movietheaters_movieid_fkey");

                entity.HasOne(mt => mt.Theater)
                      .WithMany(t => t.MovieTheaters)
                      .HasForeignKey(mt => mt.Theaterid)
                      .HasConstraintName("movietheaters_theaterid_fkey");
            });
        }
    }
}
