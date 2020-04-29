using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
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

                const string konversasjonsId = "2049057a-9b53-41bb-9cc3-d10f55fa0f87";
                const string meldingsId = "7142d8ab-9408-4cb5-8b80-dca3618dd722";
                const string tidspunkt = "2015-11-10T08:26:49.797+01:00";
                
                IntegrasjonspunktKvittering ipkvittering = new IntegrasjonspunktKvittering(
                    1L, 
                    DateTime.Parse(tidspunkt), 
                    IntegrasjonspunktKvitteringType.FEIL, 
                    "", 
                    xml.InnerXml, 
                    Guid.Parse(konversasjonsId), 
                    Guid.Parse(meldingsId), 
                    1L);
                
                //Act
                var kvittering = KvitteringFactory.GetKvittering(ipkvittering);

                //Assert
                Assert.IsType<Feilmelding>(kvittering);
            }

            [Fact]
            public void ReturnererLeveringskvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Forretningskvittering.LeveringskvitteringXml();

                const string konversasjonsId = "2049057a-9b53-41bb-9cc3-d10f55fa0f87";
                const string meldingsId = "7142d8ab-9408-4cb5-8b80-dca3618dd722";
                const string tidspunkt = "2015-11-10T08:26:49.797+01:00";
                
                IntegrasjonspunktKvittering ipkvittering = new IntegrasjonspunktKvittering(
                    1L, 
                    DateTime.Parse(tidspunkt), 
                    IntegrasjonspunktKvitteringType.LEVERT, 
                    "", 
                    xml.InnerXml, 
                    Guid.Parse(konversasjonsId), 
                    Guid.Parse(meldingsId), 
                    1L);
                    
                //Act
                var kvittering = KvitteringFactory.GetKvittering(ipkvittering);

                //Assert
                Assert.IsType<Leveringskvittering>(kvittering);
            }

            [Fact]
            public void ReturnererMottakskvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Forretningskvittering.MottakskvitteringXml();

                const string konversasjonsId = "2049057a-9b53-41bb-9cc3-d10f55fa0f87";
                const string meldingsId = "7142d8ab-9408-4cb5-8b80-dca3618dd722";
                const string tidspunkt = "2015-11-10T08:26:49.797+01:00";
                
                IntegrasjonspunktKvittering ipkvittering = new IntegrasjonspunktKvittering(
                    1L, 
                    DateTime.Parse(tidspunkt), 
                    IntegrasjonspunktKvitteringType.MOTTATT, 
                    "", 
                    xml.InnerXml, 
                    Guid.Parse(konversasjonsId), 
                    Guid.Parse(meldingsId), 
                    1L);
                
                //Act
                var kvittering = KvitteringFactory.GetKvittering(ipkvittering);

                //Assert
                Assert.IsType<Mottakskvittering>(kvittering);
            }

            [Fact]
            public void ReturnererReturpostkvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Forretningskvittering.ReturpostkvitteringXml();

                const string konversasjonsId = "2049057a-9b53-41bb-9cc3-d10f55fa0f87";
                const string meldingsId = "7142d8ab-9408-4cb5-8b80-dca3618dd722";
                const string tidspunkt = "2015-11-10T08:26:49.797+01:00";
                
                IntegrasjonspunktKvittering ipkvittering = new IntegrasjonspunktKvittering(
                    1L, 
                    DateTime.Parse(tidspunkt), 
                    IntegrasjonspunktKvitteringType.ANNET, 
                    "", 
                    xml.InnerXml, 
                    Guid.Parse(konversasjonsId), 
                    Guid.Parse(meldingsId), 
                    1L);
                
                //Act
                var kvittering = KvitteringFactory.GetKvittering(ipkvittering);

                //Assert
                Assert.IsType<Returpostkvittering>(kvittering);
            }

            [Fact]
            public void ReturnererVarslingFeiletKvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Forretningskvittering.VarslingFeiletKvitteringXml();

                const string konversasjonsId = "2049057a-9b53-41bb-9cc3-d10f55fa0f87";
                const string meldingsId = "7142d8ab-9408-4cb5-8b80-dca3618dd722";
                const string tidspunkt = "2015-11-10T08:26:49.797+01:00";
                
                IntegrasjonspunktKvittering ipkvittering = new IntegrasjonspunktKvittering(
                    1L, 
                    DateTime.Parse(tidspunkt), 
                    IntegrasjonspunktKvitteringType.FEIL, 
                    "", 
                    xml.InnerXml, 
                    Guid.Parse(konversasjonsId), 
                    Guid.Parse(meldingsId), 
                    1L);
                
                //Act
                var kvittering = KvitteringFactory.GetKvittering(ipkvittering);

                //Assert
                Assert.IsType<VarslingFeiletKvittering>(kvittering);
            }

            [Fact]
            public void ReturnererÅpningskvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Forretningskvittering.ÅpningskvitteringXml();

                const string konversasjonsId = "2049057a-9b53-41bb-9cc3-d10f55fa0f87";
                const string meldingsId = "7142d8ab-9408-4cb5-8b80-dca3618dd722";
                const string tidspunkt = "2015-11-10T08:26:49.797+01:00";
                
                IntegrasjonspunktKvittering ipkvittering = new IntegrasjonspunktKvittering(
                    1L, 
                    DateTime.Parse(tidspunkt), 
                    IntegrasjonspunktKvitteringType.LEST, 
                    "", 
                    xml.InnerXml, 
                    Guid.Parse(konversasjonsId), 
                    Guid.Parse(meldingsId), 
                    1L);
                
                //Act
                var kvittering = KvitteringFactory.GetKvittering(ipkvittering);

                //Assert
                Assert.IsType<Åpningskvittering>(kvittering);
            }
        }
    }
}
