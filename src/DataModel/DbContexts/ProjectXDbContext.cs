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

        public virtual DbSet<ActionPermission> ActionPermission { get; set; }
        public virtual DbSet<ControllerAction> ControllerAction { get; set; }
        public virtual DbSet<ToDoTask> ToDoTask { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserAction> UserAction { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<UserActionView> UserActionView { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActionPermission>(entity =>
            {
                entity.ToTable("ActionPermission", "ProjectX");

                entity.Property(e => e.PermissionName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<ControllerAction>(entity =>
            {
                entity.ToTable("ControllerAction", "ProjectX");

                entity.Property(e => e.ActionName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

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
                    .OnDelete(DeleteBehavior.Restrict)
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
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_User_RoleId_To_Role");
            });

            modelBuilder.Entity<UserAction>(entity =>
            {
                entity.ToTable("UserAction", "ProjectX");

                entity.HasIndex(e => e.ActionId)
                    .HasName("FK_UserAction_ActionId_To_Action_idx");

                entity.HasIndex(e => e.ActionPermissionId)
                    .HasName("FK_UserAction_ActionPermissionId_To_ActionPermission_idx");

                entity.HasIndex(e => e.UserRoleId)
                    .HasName("FK_UserAction_UserRoleId_To_UserRole_idx");

                entity.HasOne(d => d.Action)
                    .WithMany(p => p.UserAction)
                    .HasForeignKey(d => d.ActionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_UserAction_ActionId_To_Action");

                entity.HasOne(d => d.ActionPermission)
                    .WithMany(p => p.UserAction)
                    .HasForeignKey(d => d.ActionPermissionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_UserAction_ActionPermissionId_To_ActionPermission");

                entity.HasOne(d => d.UserRole)
                    .WithMany(p => p.UserAction)
                    .HasForeignKey(d => d.UserRoleId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_UserAction_UserRoleId_To_UserRole");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRole", "ProjectX");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UserActionView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("useractionview", "ProjectX");

                entity.Property(e => e.ActionName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
