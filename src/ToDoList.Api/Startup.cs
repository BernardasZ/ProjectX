using DataModel.DbContexts;
using DataModel.Entities;
using DataModel.Entities.ProjectX;
using DataModel.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySql.Data.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Api.Helpers;

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
			var optionManager = new OptionsManager();
			Configuration.Bind(optionManager);

			services.Configure<OptionsManager>(Configuration);

			services.AddEntityFrameworkMySQL().AddDbContext<ProjectXDbContext>(options => options.UseMySQL(optionManager.ConnectionStrings.ProjectXConnectionString), ServiceLifetime.Scoped);

			services.AddTransient(typeof(IRepository<UserRole>), typeof(Repository<UserRole, ProjectXDbContext>));
			services.AddTransient(typeof(IRepository<User>), typeof(Repository<User, ProjectXDbContext>));
			services.AddTransient(typeof(IRepository<ToDoTask>), typeof(Repository<ToDoTask, ProjectXDbContext>));

			services.AddControllers();
			services.AddSwaggerGen(x => 
			{ 
				x.EnableAnnotations(); 
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

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
