using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Api;

public class Program
{
	public static void Main(string[] args)
	{
		try
		{
			Log.Information("Application Starting.");
			CreateHostBuilder(args).Build().Run();
		}
		catch (Exception ex)
		{
			Log.Fatal(ex, "The Application failed to start.");
		}
		finally
		{
			Log.CloseAndFlush();
		}
	}

	public static void CreateLogger() =>
		Log.Logger = new LoggerConfiguration()
			.ReadFrom.Configuration(new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.Build())
			.CreateLogger();

	public static IHostBuilder CreateHostBuilder(string[] args) =>
		Host.CreateDefaultBuilder(args)
			.UseSerilog()
			.ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStartup<Startup>();
			});
}