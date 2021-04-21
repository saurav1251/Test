using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Test.Data.Models;

#nullable disable

namespace Test.Data.Context
{
    public partial class HRISContext : DataContext
    {
        public HRISContext()
        {
        }

        public HRISContext(DbContextOptions<HRISContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblRole> TblRole { get; set; }
        public virtual DbSet<TblUser> TblUser { get; set; }
        public virtual DbSet<TblUserRoleMapping> TblUserRoleMapping { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=172.29.57.203;Database=postgres;Username=postgres;Password=Microsoft@1234");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("adminpack");

            modelBuilder.Entity<TblRole>(entity =>
            {
                entity.ToTable("tbl_role", "test");

                entity.HasIndex(e => e.RoleName, "uq_role_rolename")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnType("bit(1)")
                    .HasColumnName("active");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("role_name");
            });

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.ToTable("tbl_user", "test");

                entity.HasIndex(e => e.UserName, "uq_user_username")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("last_name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("password");

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("password_salt");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("user_name");
            });

            modelBuilder.Entity<TblUserRoleMapping>(entity =>
            {
                entity.ToTable("tbl_user_role_mapping", "test");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TblUserRoleMapping)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_userrolemapping_roleid");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblUserRoleMapping)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_userrolemapping_userid");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
