using DataModel.Entities.ProjectX;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Api.Authorization;
using ToDoList.Api.Constants;
using ToDoList.Api.Helpers;
using ToDoList.Api.Services;
using ToDoList.Api.Services.Concrete;

namespace ToDoList.Api;

public static class ServiceRegistration
{
	public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration) =>
		services.AddAuthentication(x =>
		{
			x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(x =>
		{
			x.RequireHttpsMetadata = false;
			x.SaveToken = true;
			x.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetValue<string>("Jwt:JWTSecret"))),
				ValidateLifetime = true,
				RequireExpirationTime = true,
				ValidateIssuer = false,
				ValidateAudience = false,
				ClockSkew = TimeSpan.Zero
			};
		});

	public static void AddAuthorization(this IServiceCollection services) =>
		services.AddAuthorization(x =>
		{
			x.AddPolicy(Permissions.CheckPermissions, policy => policy.Requirements.Add(new ActionPermissionRequirement()));
		});

	public static void AddLocalServices(this IServiceCollection services)
	{
		services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
		services.AddSingleton<IHashCryptoHelper, HashCryptoHelper>();
		services.AddScoped<IUserServiceValidationHelper, UserServiceValidationHelper>();
		services.AddScoped<ITaskServiceValidationHelper, TaskServiceValidationHelper>();
		services.AddScoped<IAesCryptoHelper, AesCryptoHelper>();
		services.AddSingleton<ICacheService<List<PermissionView>>, PermissionCacheService>();
		services.AddScoped<IUserPermissionService, UserPermissionService>();
		services.AddScoped<IAuthorizationHandler, ActionPermissionAuthorizationHandler>();
		services.AddScoped<IUserSessionService, UserSessionService>();
		services.AddScoped<IClientContextScraper, ClientContextScraper>();
		services.AddScoped<IJwtHelper, JwtHelper>();
		services.AddScoped<IUserService, UserService>();
		services.AddScoped<ITaskService, TaskService>();
		services.AddScoped<IMessageService, MessageService>();
		services.AddScoped<IUserLoginService, UserLoginService>();
	}

	public static void AddSwagger(this IServiceCollection services) =>
		services.AddSwaggerGen(x =>
		{
			x.EnableAnnotations();
			x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				Description = "JWT Authorization header using the Bearer scheme",
				Name = "Authorization",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.ApiKey,
				Scheme = "Bearer"
			});

			x.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					Array.Empty<string>()
				}
			});
		});
}
