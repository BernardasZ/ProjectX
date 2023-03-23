using Application.Database.DbContexts;
using Domain.Models;
using Infrastructure.Databases.ProjectX.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Databases.ProjectX;

public class ProjectXDbContext : DbContext, IProjectXDbContext
{
	public ProjectXDbContext(DbContextOptions<ProjectXDbContext> options)
		: base(options)
	{
	}

	public virtual DbSet<PermissionActionModel> PermissionActions { get; set; }

	public virtual DbSet<PermissionControllerModel> PermissionControllers { get; set; }

	public virtual DbSet<PermissionMappingModel> PermissionMappings { get; set; }

	public virtual DbSet<RoleModel> Roles { get; set; }

	public virtual DbSet<TaskModel> Tasks { get; set; }

	public virtual DbSet<UserModel> Users { get; set; }

	public virtual DbSet<UserSessionModel> UserSessions { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new PermissionActionConfiguration());
		modelBuilder.ApplyConfiguration(new PermissionControllerConfiguration());
		modelBuilder.ApplyConfiguration(new PermissionMappingConfiguration());
		modelBuilder.ApplyConfiguration(new RoleConfiguration());
		modelBuilder.ApplyConfiguration(new TaskConfiguration());
		modelBuilder.ApplyConfiguration(new UserConfiguration());
		modelBuilder.ApplyConfiguration(new UserSessionConfiguration());

		base.OnModelCreating(modelBuilder);
	}
}