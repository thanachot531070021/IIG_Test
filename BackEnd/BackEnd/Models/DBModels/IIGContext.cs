using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BackEnd.Models.DBModels
{
    public partial class IIGContext : DbContext
    {
        public IIGContext()
        {
        }

        public IIGContext(DbContextOptions<IIGContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<MemberHistoryPassWord> MemberHistoryPassWords { get; set; }
        public virtual DbSet<MemberImage> MemberImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Thai_CI_AS");

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("Member");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<MemberHistoryPassWord>(entity =>
            {
                entity.ToTable("MemberHistoryPassWord");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<MemberImage>(entity =>
            {
                entity.ToTable("MemberImage");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.FileType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
