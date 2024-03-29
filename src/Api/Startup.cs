using System.Text.Json.Serialization;
using Api.Mappers;
using Api.Middleware;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
		services.AddMemoryCache();
		services.AddApiServices(Configuration);
		services.AddInfrastructure(Configuration);
		services.AddApplication(Configuration);
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
			c.DisplayRequestDuration();
		});

		app.UseForwardedHeaders(new ForwardedHeadersOptions
		{
			ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
		});

		app.UseRouting();
		app.UseErrorHandlerMiddleware();
		app.UseHttpsRedirection();
		app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
		app.UseAuthentication();
		app.UseAuthorization();

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllers();
		});
	}
}