using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Common.Logging;

/// <summary>
/// Extension methods for configuring logging services.
/// </summary>
public static class ServiceCollectionEx
{
    /// <summary>
    /// Adds logging services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddLoggingServices(this IServiceCollection services, IConfigurationRoot configuration)
    {
        return services 
            .AddLogging(logging =>
            {
                var section = configuration.GetSection("CustomLogging"); // Custom logging section
                var serilogLogger = new LoggerConfiguration() // Serilog logger configuration
                    .Enrich.WithProperty("Project", section.GetSection("Project").Value) // Enrich with project name
                    .WriteTo.Seq( // Write to Seq
                        serverUrl: section.GetSection("SeqUri").Value ?? "", // Seq server URL
                        restrictedToMinimumLevel: (LogEventLevel)Enum.Parse(typeof(LogEventLevel), section.GetSection("LogEventLevel").Value ?? "")) // Minimum log level
                    .CreateLogger(); // Create logger
                logging.AddSerilog(logger: serilogLogger, dispose: true); // Add Serilog to logging
            });
    }
}