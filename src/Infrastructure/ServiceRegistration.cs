using Application.Database.DbContexts;
using Application.Messages;
using Infrastructure.Databases.ProjectX;
using Infrastructure.EmailMessage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceRegistration
{
	public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddScoped<IDbContextBase, ProjectXDbContext>();

		services.AddProjectXDbContext(configuration);
		services.AddMessagingService();
	}

	private static void AddProjectXDbContext(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<IProjectXDbContext, ProjectXDbContext>(contextOptions =>
			contextOptions.UseSqlServer(
				configuration.GetConnectionString("ProjectXConnectionString"),
				serverOptions => serverOptions.MigrationsAssembly("MigrationsProjextX")));
	}

	private static void AddMessagingService(this IServiceCollection services)
	{
		services.AddScoped<IMessageService, MessageService>();
	}
}