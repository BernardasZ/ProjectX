using Microsoft.EntityFrameworkCore;
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

        public virtual DbSet<PermissionAction> PermissionAction { get; set; }
        public virtual DbSet<PermissionController> PermissionController { get; set; }
        public virtual DbSet<PermissionMapping> PermissionMapping { get; set; }
        public virtual DbSet<PermissionView> PermissionView { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<UserData> UserData { get; set; }
        public virtual DbSet<UserSession> UserSession { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PermissionAction>(entity =>
            {
                entity.HasIndex(e => e.ControllerId)
                    .HasName("permission_action__permission_controller_idx");

                entity.HasOne(d => d.Controller)
                    .WithMany(p => p.PermissionAction)
                    .HasForeignKey(d => d.ControllerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("permission_action__permission_controller");
            });

            modelBuilder.Entity<PermissionMapping>(entity =>
            {
                entity.HasIndex(e => e.ActionId)
                    .HasName("permission_mapping__permission_action_idx");

                entity.HasIndex(e => e.ControllerId)
                    .HasName("permission_mapping__permission_controller_idx");

                entity.HasIndex(e => e.RoleId)
                    .HasName("permission_mapping__role_idx");

                entity.HasOne(d => d.Action)
                    .WithMany(p => p.PermissionMapping)
                    .HasForeignKey(d => d.ActionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("permission_mapping__permission_action");

                entity.HasOne(d => d.Controller)
                    .WithMany(p => p.PermissionMapping)
                    .HasForeignKey(d => d.ControllerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("permission_mapping__permission_controller");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.PermissionMapping)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("permission_mapping__role");
            });

            modelBuilder.Entity<PermissionView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("permission_view", "ProjectX");
            });

            modelBuilder.Entity<UserData>(entity =>
            {
                entity.HasIndex(e => e.RoleId)
                    .HasName("user_data__role_idx");

                entity.HasIndex(e => e.UserEmail)
                    .HasName("Email_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.UserName)
                    .HasName("UserName_UNIQUE")
                    .IsUnique();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserData)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("user_data__role");
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
