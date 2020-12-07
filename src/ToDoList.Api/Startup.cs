using DataModel.DbContexts;
using DataModel.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySql.Data.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Api.Authorization;
using ToDoList.Api.Helpers;
using ToDoList.Api.Services;
using ToDoList.Api.Services.Concrete;
using static ToDoList.Api.Constants.Permissions;

namespace ToDoList.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<OptionManager>(Configuration);

			services.AddMemoryCache();
			services.AddDbContext<ProjectXDbContext>(x => x.UseMySQL(Configuration.GetConnectionString("ProjectXConnectionString")), ServiceLifetime.Scoped);
			services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

			services.AddControllers();
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

			services.AddAuthorization(x =>
			{
				x.AddPolicy(CheckPermissions, policy => policy.Requirements.Add(new ActionPermissionRequirement()));
			});

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddScoped<IAesCryptoHelper, AesCryptoHelper>();
			services.AddScoped<ICacheService<List<DataModel.Entities.ProjectX.PermissionView>>, PermissionCacheService>();
			services.AddScoped<IUserPermissionService, UserPermissionService>();
			services.AddScoped<IAuthorizationHandler, ActionPermissionAuthorizationHandler>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IClientContextScraper, ClientContextScraper>();

			var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("AppSettings:JWTSecret"));

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
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateLifetime = true,
					RequireExpirationTime = true,
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDoTasks.Api");
			});

			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
			});

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
