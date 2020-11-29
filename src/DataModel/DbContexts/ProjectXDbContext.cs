using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DataModel.Entities.ProjectX;

namespace DataModel.DbContexts
{
    public partial class ProjectXDbContext : DbContext
    {
        public ProjectXDbContext()
        {
        }

        public ProjectXDbContext(DbContextOptions<ProjectXDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ToDoTask> ToDoTask { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ToDoTask>(entity =>
            {
                entity.ToTable("ToDoTask", "ProjectX");

                entity.HasIndex(e => e.Id)
                    .HasName("idToDoTask_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.UserId)
                    .HasName("FK_ToDoTask_UserId_To_User_idx");

                entity.Property(e => e.Status).HasDefaultValueSql("'1'");

                entity.Property(e => e.TaskName)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ToDoTask)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ToDoTask_UserId_To_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "ProjectX");

                entity.HasIndex(e => e.Email)
                    .HasName("Email_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.RoleId)
                    .HasName("FK_User_RoleId_To_Role_idx");

                entity.HasIndex(e => e.UserName)
                    .HasName("UserName_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.UserName).HasMaxLength(255);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_RoleId_To_Role");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRole", "ProjectX");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}
