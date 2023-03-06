using DataModel.DbContexts;
using DataModel.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataModel;

public static class ServiceRegistration
{
	public static void AddMySqlDbContext(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<ProjectXDbContext>(x =>
			x.UseMySQL(
				configuration.GetConnectionString("ProjectXConnectionString")),
				ServiceLifetime.Scoped);

		services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
	}
}
