using System;
using System.IO;
using System.Linq;
using Difi.SikkerDigitalPost.Klient.Tester.Api;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    public class KlientkonfigurasjonTests
    {
        public class KonstruktørMethod : KlientkonfigurasjonTests
        {
            [Fact]
            public void InitializesProperties()
            {
                //Arrange
                var environment = SikkerDigitalPostKlientTests.IntegrasjonsPunktLocalHostMiljø;
                const string organizationNumberPosten = "984661185";
                object proxyhost = null;
                const string proxyScheme = "https";
                var timeoutInMilliseconds = (int) TimeSpan.FromSeconds(30).TotalMilliseconds;
                const int proxyPort = 0;

                //Act
                var clientConfiguration = new Klientkonfigurasjon(environment);

                //Assert
                Assert.Equal(environment, clientConfiguration.Miljø);
                Assert.Equal(organizationNumberPosten, clientConfiguration.MeldingsformidlerOrganisasjon.Verdi);
                Assert.Equal(proxyhost, clientConfiguration.ProxyHost);
                Assert.Equal(proxyScheme, clientConfiguration.ProxyScheme);
                Assert.Equal(timeoutInMilliseconds, clientConfiguration.TimeoutIMillisekunder);
                Assert.Equal(proxyPort, clientConfiguration.ProxyPort);
                Assert.Equal(clientConfiguration.LoggForespørselOgRespons, false);
            }
        }

    }
}
