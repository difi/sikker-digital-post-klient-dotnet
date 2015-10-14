using System;
using System.Dynamic;
using System.Linq;
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
        public class Buildmethod : X509ChainTests
        {
            [TestMethod]
            public void DetektererUtgåttSertifkat()
            {
                var utgåttSertifikat =
                    new X509Certificate2(ResourceUtility.ReadAllBytes(true, "enhetstester", "utgått.pem"));

                //Arrange
                var ignoreStoreMySertifikater = true;
                var chain = new X509Chain(ignoreStoreMySertifikater);

                //Act
                chain.Build(utgåttSertifikat);

                //Assert
                Assert.Equals(X509ChainStatusFlags.NotTimeValid, chain.ChainStatus.Select(e => e.Status == X509ChainStatusFlags.NotTimeValid));
            }

            [TestMethod]
            public void GyldigKjedeUtenRevokeringssjekkOgUkjentCertificateAuthority()
            {
                var gyldigSertifikat = new X509Certificate2(ResourceUtility.ReadAllBytes(true,"test", "testmottakerFraOppslagstjenesten.pem"));

                //Arrange
                var ignoreStoreMySertifikater = true;
                var chain = new X509Chain(ignoreStoreMySertifikater)
                {
                    ChainPolicy = ChainPolicyForTest
                };

                //Act
                var isValidCertificate = chain.Build(gyldigSertifikat);

                //Assert
                Assert.IsTrue(isValidCertificate);
            }

            public X509ChainPolicy ChainPolicyForProd
            {
                get
                {
                    return new X509ChainPolicy()
                    {
                        RevocationFlag = X509RevocationFlag.EntireChain,
                        RevocationMode = X509RevocationMode.Online,
                        UrlRetrievalTimeout = new TimeSpan(0, 1, 0),
                        VerificationFlags = X509VerificationFlags.NoFlag,
                        ExtraStore =
                        {
                            new X509Certificate2(ResourceUtility.ReadAllBytes(true, "prod", "BPClass3CA3.cer")),
                            new X509Certificate2(ResourceUtility.ReadAllBytes(true, "prod", "BPClass3RootCA.cer")),
                            new X509Certificate2(ResourceUtility.ReadAllBytes(true, "prod", "cpn enterprise sha256 class 3.crt")),
                            new X509Certificate2(ResourceUtility.ReadAllBytes(true, "prod", "cpn rootca sha256 class 3.crt"))
                        }
                    };
                }
            }
            
            public X509ChainPolicy ChainPolicyForTest
            {
                get
                {
                    return new X509ChainPolicy()
                    {
                        RevocationMode = X509RevocationMode.NoCheck,
                        UrlRetrievalTimeout = new TimeSpan(0, 1, 0),
                        VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority,
                        ExtraStore =
                        {
                            new X509Certificate2(ResourceUtility.ReadAllBytes(true, "test", "Buypass_Class_3_Test4_CA_3.cer")),
                            new X509Certificate2(ResourceUtility.ReadAllBytes(true, "test", "Buypass_Class_3_Test4_Root_CA.cer")),
                            new X509Certificate2(ResourceUtility.ReadAllBytes(true, "test", "intermediate - commfides cpn enterprise-norwegian sha256 ca - test2.crt")),
                            new X509Certificate2(ResourceUtility.ReadAllBytes(true, "test","root - cpn root sha256 ca - test.crt"))
                        }
                    };
                }
            }
        }
    }
}
