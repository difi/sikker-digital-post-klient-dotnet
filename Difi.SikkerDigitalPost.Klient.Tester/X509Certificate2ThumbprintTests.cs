using System;
using System.Security.Cryptography.X509Certificates;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    public class X509Certificate2ThumbprintTests
    {
        public class ThumbprintTester : IDisposable
        {
            private static X509Store _store;
            private static X509Certificate2 _certificate;

            public ThumbprintTester()
            {
                _store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                _store.Open(OpenFlags.ReadOnly);

                try
                {
                    _certificate = _store.Certificates[0];
                }
                catch
                {
                    throw new ArgumentNullException("Klarte ikke å finne noen sertifikater til å gjøre tester på. Dette er nok fordi du ikke har noen sertifikater i CurrentUser.My.");
                }
            }

            public void Dispose()
            {
                _store.Close();
            }

            [Fact(Skip = "Skipping because we don't use thumbprints anymore. Leaving so I can confirm that before deleting entirely.")]
            public void TestLowercaseThumbprint()
            {
                var lowercaseThumbprint = _certificate.Thumbprint.ToLower();
                var certificateFound = _store.Certificates.Find(X509FindType.FindByThumbprint,
                    lowercaseThumbprint, false)[0];

                Assert.True(_certificate.Equals(certificateFound),
                    "Sertifikat funnet med thumbprint matcher ikke referansesertifikat");
            }

            [Fact(Skip = "Skipping because we don't use thumbprints anymore. Leaving so I can confirm that before deleting entirely.")]
            public void TestRandomSpacingThumbprint()
            {
                var randomSpacingThumb = string.Empty;

                var random = new Random();
                for (var i = 0; i < _certificate.Thumbprint.Length; i++)
                {
                    if (i%random.Next(1, 5) == 0 && i > 0)
                        randomSpacingThumb += " ";

                    randomSpacingThumb += _certificate.Thumbprint[i];
                }

                var certificateFound = _store.Certificates.Find(X509FindType.FindByThumbprint,
                    randomSpacingThumb, false)[0];

                Assert.True(_certificate.Equals(certificateFound),
                    "Sertifikat funnet med thumbprint matcher ikke referansesertifikat");
            }
        }
    }
}