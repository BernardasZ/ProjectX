using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Database.DbContexts;

public interface IProjectXDbContext : IDbContextBase
{
	DbSet<PermissionActionModel> PermissionActions { get; set; }

	DbSet<PermissionControllerModel> PermissionControllers { get; set; }

	DbSet<PermissionMappingModel> PermissionMappings { get; set; }

	DbSet<RoleModel> Roles { get; set; }

	DbSet<TaskModel> Tasks { get; set; }

	DbSet<UserModel> Users { get; set; }

	DbSet<UserSessionModel> UserSessions { get; set; }
}