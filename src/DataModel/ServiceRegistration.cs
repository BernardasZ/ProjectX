using DataModel.DbContexts;
using DataModel.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace DataModel;

public static class ServiceRegistration
{
	public static void AddMySqlDbContext(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<ProjectXDbContext>(x =>
		x.UseMySql(
			configuration.GetConnectionString("ProjectXConnectionString"),
			ServerVersion.Create(1, 1, 1, ServerType.MySql)));

		services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
	}
}