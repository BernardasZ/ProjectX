using Api.Mappers;
using Api.Middleware;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;

namespace Api;

public class Startup
{
	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	public static IConfiguration Configuration { get; set; }

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddControllers()
			.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
		services.AddSwagger();
		services.AddMemoryCache();
		services.AddHttpContextAccessor();
		services.AddInfrastructure(Configuration);
		services.AddApplication(Configuration);
		services.AddLocalServices();
		services.AddAuthentication(Configuration);
		services.AddAuthorization();
		services.AddAutoMapper(typeof(ApiMapperProfile));
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}

		app.UsePathBase("/api/v1");
		app.UseSwagger();
		app.UseSwaggerUI(c =>
		{
			c.SwaggerEndpoint("v1/swagger.json", "ToDoTasks.Api");
			c.RoutePrefix = "swagger";
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