using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using Difi.Felles.Utility.Utilities;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    public class X509ChainTests
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Tester.testdata.sertifikater");

        public class Buildmethod : X509ChainTests
        {
            public X509ChainPolicy ChainPolicyUtenRevokeringssjekkOgUkjentCertificateAuthority
            {
                get
                {
                    var policy = new X509ChainPolicy
                    {
                        RevocationMode = X509RevocationMode.NoCheck,
                        UrlRetrievalTimeout = new TimeSpan(0, 1, 0),
                        VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority
                    };
                    policy.ExtraStore.AddRange(CertificateChainUtility.FunksjoneltTestmiljøSertifikater());

                    return policy;
                }
            }

            public X509ChainPolicy ChainPolicyWithOnlineCheckOgUkjentRotnode
            {
                get
                {
                    var policy = new X509ChainPolicy
                    {
                        RevocationFlag = X509RevocationFlag.EntireChain,
                        RevocationMode = X509RevocationMode.Online,
                        UrlRetrievalTimeout = new TimeSpan(0, 1, 0),
                        VerificationFlags = X509VerificationFlags.NoFlag
                    };
                    policy.ExtraStore.AddRange(CertificateChainUtility.FunksjoneltTestmiljøSertifikater());

                    return policy;
                }
            }

            [Fact]
            public void DetektererUtgåttSertifkat()
            {
                var utgåttSertifikat =
                    new X509Certificate2(ResourceUtility.ReadAllBytes(true, "enhetstester", "utgått.pem"));

                //Arrange
                const bool ignoreStoreMySertifikater = true;
                var chain = new X509Chain(ignoreStoreMySertifikater);

                //Act
                chain.Build(utgåttSertifikat);

                //Assert
                Assert.True(chain.ChainStatus.Select(e => e.Status == X509ChainStatusFlags.NotTimeValid).Any());
            }

            [Fact]
            public void GyldigKjedeUtenRevokeringssjekkOgUkjentCertificateAuthority()
            {
                var gyldigSertifikat = new X509Certificate2(ResourceUtility.ReadAllBytes(true, "test", "testmottakersertifikatFraOppslagstjenesten.pem"));

                //Arrange
                const bool ignoreStoreMySertifikater = true;
                var chain = new X509Chain(ignoreStoreMySertifikater)
                {
                    ChainPolicy = ChainPolicyUtenRevokeringssjekkOgUkjentCertificateAuthority
                };

                //Act
                var isValidCertificate = chain.Build(gyldigSertifikat);

                //Assert
                Assert.True(isValidCertificate);
            }

            [Fact]
            public void GyldigKjedeMedUkjentRotnodeOgUgyldigOnlineOppslag()
            {
                //Arrange
                var gyldigSertifikat = new X509Certificate2(ResourceUtility.ReadAllBytes(true, "test", "testmottakersertifikatFraOppslagstjenesten.pem"));
                const bool ignoreStoreMySertifikater = true;
                var chain = new X509Chain(ignoreStoreMySertifikater)
                {
                    ChainPolicy = ChainPolicyWithOnlineCheckOgUkjentRotnode
                };

                //Act
                chain.Build(gyldigSertifikat);
                var chainElements = new X509ChainElement[chain.ChainElements.Count];
                chain.ChainElements.CopyTo(chainElements, 0);

                //Assert
                var elementerMedRevokeringsstatusUkjent = chainElements.Select(chainElement => new
                {
                    Status = chainElement.ChainElementStatus
                        .Where(elementStatus => elementStatus.Status == X509ChainStatusFlags.RevocationStatusUnknown)
                }).Where(node => node.Status.Any());
                Assert.Equal(2, elementerMedRevokeringsstatusUkjent.Count());

                var rotNode = chainElements[0];
                Assert.Equal(0, rotNode.ChainElementStatus.Count(elementStatus => elementStatus.Status == X509ChainStatusFlags.UntrustedRoot));
            }
        }
    }
}