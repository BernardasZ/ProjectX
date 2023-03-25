using Application.Database.DbContexts;
using Application.Messages;
using Infrastructure.Databases.Options;
using Infrastructure.Databases.ProjectX;
using Infrastructure.EmailMessage;
using Infrastructure.EmailMessage.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceRegistration
{
	public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionstrings = new ConnectionStrings();
		configuration.GetSection(ConnectionStrings.SelectionName).Bind(connectionstrings);

		services.Configure<SmtpSettings>(configuration);
		services.AddScoped<IDbContextBase, ProjectXDbContext>();

		services.AddProjectXDbContext(connectionstrings.ProjectXConnectionString);
		services.AddMessagingService();
	}

	private static void AddProjectXDbContext(this IServiceCollection services, string connectionString) =>
		services.AddDbContext<IProjectXDbContext, ProjectXDbContext>(contextOptions =>
			contextOptions.UseSqlServer(
				connectionString,
				serverOptions => serverOptions.MigrationsAssembly("MigrationsProjextX")));

	private static void AddMessagingService(this IServiceCollection services) => services.AddScoped<IMessageService, MessageService>();
}