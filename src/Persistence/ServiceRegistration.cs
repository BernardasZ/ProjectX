using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.DbContexts;
using Persistence.Repositories;

namespace DataModel;

public static class ServiceRegistration
{
	public static void AddMySqlDbContext(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<ProjectXDbContext>(options =>
			options.UseSqlServer(configuration.GetConnectionString("ProjectXConnectionString")));

		services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
		services.AddScoped<IPermissionMappingRepository, PermissionMappingRepository>();
	}
}