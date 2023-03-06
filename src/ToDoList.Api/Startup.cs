using DataModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;
using ToDoList.Api.Helpers;
using ToDoList.Api.Middleware;

namespace ToDoList.Api;

public class Startup
{
	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	public static IConfiguration Configuration { get; set; }

	public void ConfigureServices(IServiceCollection services)
	{
		services.Configure<OptionManager>(Configuration);
		services.AddMemoryCache();
		services.AddMySqlDbContext(Configuration);
		services.AddControllers()
			.AddJsonOptions(x => x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault);
		services.AddSwagger();
		services.AddAuthorization();
		services.AddLocalServices();
		services.AddAuthentication(Configuration);
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
		app.UseErrorHandlerMiddleware();
		app.UseAuthentication();
		app.UseAuthorization();

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllers();
		});
	}
}
