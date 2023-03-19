using Microsoft.EntityFrameworkCore;
using Persistence.Entities.ProjectX;
using Persistence.Seeds;

namespace Persistence.DbContexts;

public class ProjectXDbContext : DbContext
{
	public ProjectXDbContext(DbContextOptions<ProjectXDbContext> options)
		: base(options)
	{
	}

	public virtual DbSet<PermissionAction> PermissionActions { get; set; }

	public virtual DbSet<PermissionController> PermissionControllers { get; set; }

	public virtual DbSet<PermissionMapping> PermissionMappings { get; set; }

	public virtual DbSet<Role> Roles { get; set; }

	public virtual DbSet<Task> Tasks { get; set; }

	public virtual DbSet<User> Users { get; set; }

	public virtual DbSet<UserSession> UserSessions { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Role>().HasData(RoleSeed.GetRoleSeed());

		modelBuilder.Entity<User>().Navigation(x => x.Role).AutoInclude();
		modelBuilder.Entity<Task>().Navigation(x => x.User).AutoInclude();
		modelBuilder.Entity<PermissionMapping>().Navigation(x => x.Role).AutoInclude();
		modelBuilder.Entity<PermissionMapping>().Navigation(x => x.Action).AutoInclude();
		modelBuilder.Entity<PermissionMapping>().Navigation(x => x.Controller).AutoInclude();

		base.OnModelCreating(modelBuilder);
	}
}