using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Api;

public class Program
{
	private const string _template = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Message}{NewLine:1}{Exception:1}";

	public static void Main(string[] args)
	{
		try
		{
			CreateLogger();

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
			.MinimumLevel.Debug()
			.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
			.Enrich.FromLogContext()
			.WriteTo.Console(outputTemplate: _template)
			.WriteTo.Debug()
			.WriteTo.File(
				"AppData\\log.txt",
				rollingInterval: RollingInterval.Infinite,
				outputTemplate: _template)
			.CreateLogger();

	public static IHostBuilder CreateHostBuilder(string[] args) =>
		Host.CreateDefaultBuilder(args)
			.ConfigureLogging(l =>
			{
				l.ClearProviders();
				l.AddSerilog();
			})
			.ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStartup<Startup>();
			});
}