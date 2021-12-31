using Microsoft.EntityFrameworkCore;

#nullable disable

namespace FraudDetection.UI.Models
{
    public partial class FrauddbContext : DbContext
    {
        public FrauddbContext(DbContextOptions<FrauddbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<SourceFile> SourceFiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SourceFile>(entity =>
            {
                entity.ToTable("source_file");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.CreateDate).HasDefaultValueSql("'NULL'");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ParentId).HasColumnType("int(11)");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
