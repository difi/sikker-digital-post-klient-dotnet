using System;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using ApiClientShared.Enums;
using Difi.SikkerDigitalPost.Klient.Tester.Properties;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    [TestClass]
    public class X509ChainTests
    {
        ResourceUtility ResourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Tester.testdata.sertifikater");
        
        [TestClass]
        public class ByukdMethod : X509ChainTests
        {
            [TestMethod]
            public void ErUgyldigSertifikatkjede()
            {
                var utgåttSertifikat = new X509Certificate2(ResourceUtility.ReadAllBytes(true, "utgått.pem"));

                //Arrange
                var chain = new X509Chain
                {
                    ChainPolicy =
                    {
                        RevocationFlag = X509RevocationFlag.ExcludeRoot,
                        RevocationMode = X509RevocationMode.Online,
                        UrlRetrievalTimeout = new TimeSpan(0, 1, 0),
                        VerificationFlags = X509VerificationFlags.NoFlag
                    }
                };

                //Act

                //Assert
                bool isValidCertificate = chain.Build(utgåttSertifikat);
                Assert.IsFalse(isValidCertificate);
            }

            [TestMethod]
            public void ErGyldigSertifikatKjede()
            {
                var gyldigSertifikatMedPk =
                    new X509Certificate2(ResourceUtility.ReadAllBytes(true, "dp-testvirksomhet.p12"));
                
                //Arrange
                var chain = new X509Chain
                {
                    ChainPolicy =
                    {
                        RevocationFlag = X509RevocationFlag.ExcludeRoot,
                        RevocationMode = X509RevocationMode.Online,
                        UrlRetrievalTimeout = new TimeSpan(0, 1, 0),
                        VerificationFlags = X509VerificationFlags.NoFlag
                    }
                };

                //Act

                //Assert
                bool isValidCertificate = chain.Build(gyldigSertifikatMedPk);
                Assert.IsTrue(isValidCertificate);
            }

        }
    }
}
