using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    [TestClass]
    public class SertifikatTester
    {
        [TestClass]
        public class ThumbprintTester 
        {
            private static X509Store _store;
            private static X509Certificate2 _certificate;

            [ClassInitialize]
            public static void ClassInitialize(TestContext context)
            {
                _store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                _store.Open(OpenFlags.ReadOnly);

                try {
                    _certificate = _store.Certificates[0]; }
                catch
                {
                    Assert.Fail("Klarte ikke å finne noen sertifikater til å gjøre tester på. " +
                                "Dette er nok fordi du ikke har noen sertifikater i CurrentUser.My.");
                }
            }

            [ClassCleanup]
            public static void ClassCleanup()
            {
                _store.Close();
            }

            [TestMethod]
            public void TestLowercaseThumbprint()
            {
                string lowercaseThumbprint = _certificate.Thumbprint.ToLower();
                var certificateFound = _store.Certificates.Find(X509FindType.FindByThumbprint,
                    lowercaseThumbprint, false)[0];

                Assert.IsTrue(_certificate.Equals(certificateFound),
                    "Sertifikat funnet med thumbprint matcher ikke referansesertifikat");
            }

            [TestMethod]
            public void TestRandomSpacingThumbprint()
            {
                var randomSpacingThumb = String.Empty;

                var random = new Random();
                for (var i = 0; i < _certificate.Thumbprint.Length; i++)
                {
                    if (i%random.Next(1, 5) == 0 && i > 0)
                        randomSpacingThumb += " ";

                    randomSpacingThumb += _certificate.Thumbprint[i];
                }

                var certificateFound = _store.Certificates.Find(X509FindType.FindByThumbprint,
                    randomSpacingThumb, false)[0];

                Assert.IsTrue(_certificate.Equals(certificateFound),
                    "Sertifikat funnet med thumbprint matcher ikke referansesertifikat");
            }
        }
    }
}
