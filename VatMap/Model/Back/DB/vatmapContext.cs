using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace VatMap.Model.Back.DB
{
    public partial class VatmapContext : DbContext
    {
        public VatmapContext()
        {
        }

        public VatmapContext(DbContextOptions<VatmapContext> options)
            : base(options)
        {
        }


    public virtual DbSet<Airport> Airport { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=VAJGL-NB-490;Initial Catalog=vatmap;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Airport>(entity =>
            {
                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Iata)
                    .HasColumnName("IATA")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Icao)
                    .IsRequired()
                    .HasColumnName("ICAO")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.TimeZoneName)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.TimeZoneSummerTimeType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });
        }
    }
}
