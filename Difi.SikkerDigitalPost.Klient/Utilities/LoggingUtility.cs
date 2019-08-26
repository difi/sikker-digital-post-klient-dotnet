using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Difi.SikkerDigitalPost.Klient.Utilities
{
    public static class LoggingUtility
    {
        internal static IServiceProvider CreateServiceProviderAndSetUpLogging()
        {
            var services = new ServiceCollection();
            
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace));

            var serviceProvider = services.BuildServiceProvider();
            SetUpLoggingForTesting(serviceProvider);

            return serviceProvider;
        }
        
        private static void SetUpLoggingForTesting(IServiceProvider serviceProvider)
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            loggerFactory.AddNLog(new NLogProviderOptions {CaptureMessageTemplates = true, CaptureMessageProperties = true});
            NLog.LogManager.LoadConfiguration("./../../../../Difi.SikkerDigitalPost.Klient/nlog.config");
        }

    }
}
