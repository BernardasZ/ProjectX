using Api.Authorization;
using Api.Constants;
using Api.Resources;
using Api.Services;
using Api.Services.Interfaces;
using Application.Authentication;
using Application.Authorization;
using Application.Services.Interfaces;
using Domain.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace Api;

public static class ServiceRegistration
{
	public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddAuthentication(configuration);
		services.AddAuthorization();
		services.AddLocalServices();
		services.AddSwagger();
	}

	private static void AddAuthentication(this IServiceCollection services, IConfiguration configuration) =>
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

	private static void AddAuthorization(this IServiceCollection services) =>
		services.AddAuthorization(x =>
		{
			x.AddPolicy(Permissions.CheckPermissions, policy => policy.Requirements.Add(new ActionPermissionRequirement()));
		});

	private static void AddLocalServices(this IServiceCollection services)
	{
		services.AddHttpContextAccessor();
		services.AddSingleton(typeof(ICacheService<>), typeof(CacheService<>));
		services.AddSingleton<IResourceManager, ApiErrorResourceManager>();
		services.AddScoped<IPermissionCacheService, PermissionCacheService>();
		services.AddScoped<IUserPermissionService, UserPermissionService>();
		services.AddScoped<IAuthorizationHandler, ActionPermissionAuthorizationHandler>();
		services.AddScoped<IClientContextScraper, ClientContextScraperService>();
		services.AddScoped<IJwtService, JwtService>();
	}

	private static void AddSwagger(this IServiceCollection services) =>
		services.AddSwaggerGen(x =>
		{
			x.EnableAnnotations();
			x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				Description = "JWT Authorization header using the Bearer scheme",
				Name = "Authorization",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.ApiKey,
				Scheme = "Bearer",
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