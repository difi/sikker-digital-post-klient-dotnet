using System;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    [TestClass]
    public class KlientkonfigurasjonTester
    {
        [TestClass]
        public class KonstruktørMethod : KlientkonfigurasjonTester
        {
            [TestMethod]
            public void EnkelKonstruktørMedMiljø()
            {
                //Arrange
                var miljø = Miljø.FunksjoneltTestmiljø;
                var organisasjonsnummerPosten = "984661185";
                object proxyhost = null;
                string proxyScheme = "https";
                var timeoutIMillisekunder = (int)TimeSpan.FromSeconds(30).TotalMilliseconds;
                var loggXmlTilFil = false;
                int proxyPort = 0;

                //Act
                Klientkonfigurasjon klientkonfigurasjon = new Klientkonfigurasjon(miljø);
                
                //Assert
                Assert.AreEqual(miljø, klientkonfigurasjon.Miljø);
                Assert.AreEqual(organisasjonsnummerPosten, klientkonfigurasjon.MeldingsformidlerOrganisasjon.Verdi);
                Assert.AreEqual(proxyhost, klientkonfigurasjon.ProxyHost);
                Assert.AreEqual(proxyScheme, klientkonfigurasjon.ProxyScheme);
                Assert.AreEqual(timeoutIMillisekunder, klientkonfigurasjon.TimeoutIMillisekunder);
                Assert.IsNotNull(klientkonfigurasjon.Logger);
                Assert.AreEqual(loggXmlTilFil, klientkonfigurasjon.LoggXmlTilFil);
                Assert.AreEqual(proxyPort, klientkonfigurasjon.ProxyPort);
            }
        }
    }
}