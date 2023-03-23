using Application.Authorization;
using Application.Database.Repositories;
using Application.Helpers.Cryptography;
using Application.Options;
using Application.Services;
using Application.Services.Interfaces;
using Application.Validations;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api;

public static class ServiceRegistration
{
	public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<Configuration>(configuration);
		services.AddScoped<IHashCryptoHelper, HashCryptoHelper>();
		services.AddScoped<IUserValidationService, UserValidationService>();
		services.AddScoped<ITaskValidationService, TaskValidationService>();
		services.AddScoped<IAesCryptoHelper, AesCryptoHelper>();
		services.AddScoped<IUserSessionService, UserSessionService>();
		services.AddScoped<IServiceBase<UserModel>, UserService>();
		services.AddScoped<ITaskService, TaskService>();
		services.AddScoped<IUserLoginService, UserLoginService>();
		services.AddScoped<IPermissionCacheService, PermissionCacheService>();
		services.AddScoped<IUserRecoverService, UserRecoverService>();
		services.AddScoped<IDateTime, DateTimeService>();
		services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
	}
}