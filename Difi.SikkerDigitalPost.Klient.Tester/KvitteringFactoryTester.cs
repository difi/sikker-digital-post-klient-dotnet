using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    public class KvitteringFactoryTester
    {
        public class GetKvitteringMethod : KvitteringFactoryTester
        {
            [Fact]
            public void ReturnererFeilmelding()
            {
                //Arrange
                var xml = KvitteringsUtility.Forretningskvittering.FeilmeldingXml();

                //Act
                var kvittering = KvitteringFactory.GetKvittering(xml);

                //Assert
                Assert.IsType<Feilmelding>(kvittering);
            }

            [Fact]
            public void ReturnererLeveringskvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Forretningskvittering.LeveringskvitteringXml();

                //Act
                var kvittering = KvitteringFactory.GetKvittering(xml);

                //Assert
                Assert.IsType<Leveringskvittering>(kvittering);
            }

            [Fact]
            public void ReturnererMottakskvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Forretningskvittering.MottakskvitteringXml();

                //Act
                var kvittering = KvitteringFactory.GetKvittering(xml);

                //Assert
                Assert.IsType<Mottakskvittering>(kvittering);
            }

            [Fact]
            public void ReturnererReturpostkvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Forretningskvittering.ReturpostkvitteringXml();

                //Act
                var kvittering = KvitteringFactory.GetKvittering(xml);

                //Assert
                Assert.IsType<Returpostkvittering>(kvittering);
            }

            [Fact]
            public void ReturnererVarslingFeiletKvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Forretningskvittering.VarslingFeiletKvitteringXml();

                //Act
                var kvittering = KvitteringFactory.GetKvittering(xml);

                //Assert
                Assert.IsType<VarslingFeiletKvittering>(kvittering);
            }

            [Fact]
            public void ReturnererÅpningskvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Forretningskvittering.ÅpningskvitteringXml();

                //Act
                var kvittering = KvitteringFactory.GetKvittering(xml);

                //Assert
                Assert.IsType<Åpningskvittering>(kvittering);
            }

            [Fact]
            public void ReturnererTomKøKvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Transportkvittering.TomKøKvitteringXml();

                //Act
                var kvittering = KvitteringFactory.GetKvittering(xml);

                //Assert
                Assert.IsType<TomKøKvittering>(kvittering);
            }

            [Fact]
            public void ReturnererTransportFeiletKvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Transportkvittering.TransportFeiletKvitteringXml();

                //Act
                var kvittering = KvitteringFactory.GetKvittering(xml);

                //Assert
                Assert.IsType<TransportFeiletKvittering>(kvittering);
            }

            [Fact]
            public void ReturnererTransportOkKvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Transportkvittering.TransportOkKvitteringXml();

                //Act
                var kvittering = KvitteringFactory.GetKvittering(xml);

                //Assert
                Assert.IsType<TransportOkKvittering>(kvittering);
            }
        }
    }
}