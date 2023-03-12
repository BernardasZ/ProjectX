using DataModel.Entities.ProjectX;
using Microsoft.EntityFrameworkCore;

namespace DataModel.DbContexts;

public class ProjectXDbContext : DbContext
{
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
		PermissionActionMapping(modelBuilder);
		PermissionMappingMapping(modelBuilder);
		PermissionViewMapping(modelBuilder);
		UserDataMapping(modelBuilder);

		base.OnModelCreating(modelBuilder);
	}

	private static void UserDataMapping(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<UserData>(entity =>
		{
			entity.HasIndex(e => e.RoleId)
				.HasDatabaseName("user_data__role_idx");

			entity.HasIndex(e => e.UserEmail)
				.HasDatabaseName("Email_UNIQUE")
				.IsUnique();

			entity.HasIndex(e => e.UserName)
				.HasDatabaseName("UserName_UNIQUE")
				.IsUnique();

			entity.HasOne(d => d.Role)
				.WithMany(p => p.UserData)
				.HasForeignKey(d => d.RoleId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("user_data__role");
		});
	}

	private static void PermissionViewMapping(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<PermissionView>(entity =>
		{
			entity.HasNoKey();

			entity.ToView("permission_view", "ProjectX");
		});
	}

	private static void PermissionMappingMapping(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<PermissionMapping>(entity =>
		{
			entity.HasIndex(e => e.ActionId)
				.HasDatabaseName("permission_mapping__permission_action_idx");

			entity.HasIndex(e => e.ControllerId)
				.HasDatabaseName("permission_mapping__permission_controller_idx");

			entity.HasIndex(e => e.RoleId)
				.HasDatabaseName("permission_mapping__role_idx");

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
	}

	private static void PermissionActionMapping(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<PermissionAction>(entity =>
		{
			entity.HasIndex(e => e.ControllerId)
				.HasDatabaseName("permission_action__permission_controller_idx");

			entity.HasOne(d => d.Controller)
				.WithMany(p => p.PermissionAction)
				.HasForeignKey(d => d.ControllerId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("permission_action__permission_controller");
		});
	}
}