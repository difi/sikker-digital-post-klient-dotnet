using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

namespace Difi.SikkerDigitalPost.Klient
{ 
    [Obsolete]
    public class CertificateReader
    {
        private readonly ILogger<CertificateReader> _logger;

        private CertificateReader(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CertificateReader>();
        }

        [Obsolete]
        public static X509Certificate2 ReadCertificate()
        {
            var certificateReader = new CertificateReader(new NullLoggerFactory());
            return certificateReader.ReadCertificatePrivate();
        }

        [Obsolete]
        public static X509Certificate2 ReadCertificate(ILoggerFactory loggerFactory)
        {
            var certificateReader = new CertificateReader(loggerFactory);
            return certificateReader.ReadCertificatePrivate();
        }
        
        [Obsolete]
        X509Certificate2 ReadCertificatePrivate()
        {
            var pathToSecrets = $"{System.Environment.GetEnvironmentVariable("HOME")}/.microsoft/usersecrets/enterprise-certificate/secrets.json";
            _logger.LogDebug($"Reading certificate details from secrets file: {pathToSecrets}");
            var fileExists = File.Exists(pathToSecrets);

            if (!fileExists)
            {
                _logger.LogDebug($"Did not find file at {pathToSecrets}");
            }
            
            var certificateConfig = File.ReadAllText(pathToSecrets);
            var deserializeObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(certificateConfig);

            deserializeObject.TryGetValue("Certificate:Path:Absolute", out var certificatePath);
            deserializeObject.TryGetValue("Certificate:Password", out var certificatePassword);

            _logger.LogDebug("Reading certificate from path found in secrets file: " + certificatePath);

            return new X509Certificate2(certificatePath, certificatePassword, X509KeyStorageFlags.Exportable);
        }
    }
}
