using DataModel.DbContexts;
using DataModel.Entities;
using DataModel.Entities.ProjectX;
using DataModel.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
using System.Text;
using System.Threading.Tasks;
using ToDoList.Api.Authorization;
using ToDoList.Api.Helpers;
using ToDoList.Api.Services;
using ToDoList.Api.Services.Concrete;
using static ToDoList.Api.Constants.Constants.UserPermissions;

namespace ToDoList.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var optionManager = new OptionManager();
			Configuration.Bind(optionManager);

			services.Configure<OptionManager>(Configuration);

			services.AddMemoryCache();
			services.AddDbContext<ProjectXDbContext>(options => options.UseMySQL(optionManager.ConnectionStrings.ProjectXConnectionString), ServiceLifetime.Scoped);

			services.AddTransient(typeof(IRepository<UserRole>), typeof(Repository<UserRole, ProjectXDbContext>));
			services.AddTransient(typeof(IRepository<User>), typeof(Repository<User, ProjectXDbContext>));
			services.AddTransient(typeof(IRepository<ToDoTask>), typeof(Repository<ToDoTask, ProjectXDbContext>));
			services.AddTransient(typeof(IRepository<UserActionView>), typeof(Repository<UserActionView, ProjectXDbContext>));

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

			services.AddAuthorization(options =>
			{
				options.AddPolicy(Create, policy => policy.Requirements.Add(new ActionPermissionRequirement(Create)));
				options.AddPolicy(Read, policy => policy.Requirements.Add(new ActionPermissionRequirement(Read)));
				options.AddPolicy(Update, policy => policy.Requirements.Add(new ActionPermissionRequirement(Update)));
				options.AddPolicy(Delete, policy => policy.Requirements.Add(new ActionPermissionRequirement(Delete)));
			});


			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddScoped<IAesCryptoService, AesCryptoService>();
			services.AddScoped<ICacheService, CacheService>();
			services.AddScoped<IUserPermissionService, UserPermissionService>();

			services.AddScoped<IAuthorizationHandler, ActionPermissionAuthorizationHandler>();

			var key = Encoding.ASCII.GetBytes(optionManager.AppSettings.JWTSecret);

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
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});



		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
